using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sienar.Errors;
using Sienar.Infrastructure;

namespace Sienar.Identity;

public class AdminAccountService<TUser, TContext>
	: CrudService<TUser, SienarUserDto, TContext>,
		IAdminAccountService
	where TUser : SienarUser, new()
	where TContext : DbContext
{
	protected readonly IPasswordHasher<TUser> PasswordHasher;
	protected readonly IHttpContextAccessor HttpContextAccessor;
	protected readonly ISignInManager<TUser> SignInManager;
	protected readonly LoginOptions LoginOptions;

	/// <inheritdoc />
	public AdminAccountService(
		TContext context,
		IDtoAdapter<TUser, SienarUserDto> adapter,
		IFilterProcessor<TUser> filterProcessor,
		IBotDetector botDetector,
		IPasswordHasher<TUser> passwordHasher,
		IHttpContextAccessor httpContextAccessor,
		ISignInManager<TUser> signInManager,
		IOptions<LoginOptions> loginOptions)
		: base(context, adapter, filterProcessor, botDetector)
	{
		PasswordHasher = passwordHasher;
		HttpContextAccessor = httpContextAccessor;
		SignInManager = signInManager;
		LoginOptions = loginOptions.Value;

		EntityName = "User";
	}

	/// <inheritdoc />
	public override async Task<ServiceResult<SienarUserDto>> Get(
		Guid id,
		Filter? filter = null)
	{
		filter ??= new Filter
		{
			Includes = new[]
			{
				"Roles"
			}
		};
		return await base.Get(id, filter);
	}

	/// <inheritdoc />
	public override Task<ServiceResult<Guid>> Add(SienarUserDto model)
	{
		UpdatePasswordIfAppropriate(model);
		return base.Add(model);
	}

	/// <inheritdoc />
	public override Task<ServiceResult<bool>> Edit(SienarUserDto model)
	{
		UpdatePasswordIfAppropriate(model);
		return base.Edit(model);
	}

	public override async Task<ServiceResult<bool>> Delete(Guid id)
	{
		var result = await base.Delete(id);

		var currentUserId = HttpContextAccessor.HttpContext!.User.Claims.First(
			c => c.Type == ClaimTypes.NameIdentifier);
		if (Guid.Parse(currentUserId.Value) == id)
		{
			await SignInManager.SignOut();
		}

		return result;
	}

	/// <inheritdoc />
	public async Task<ServiceResult<bool>> AddUserToRole(UserUpdateRolesDto data)
	{
		var user = await GetUserWithRoles(data.UserId);

		if (user.Roles.Any(r => r.Id == data.RoleId))
		{
			return ServiceResult<bool>.Unprocessable(ErrorMessages.Account.AccountAlreadyInRole);
		}

		var role = await Context
			.Set<SienarRole>()
			.FindAsync(data.RoleId);
		if (role is null)
		{
			return ServiceResult<bool>.NotFound(ErrorMessages.Roles.NotFound);
		}

		user.Roles.Add(role);
		await Context.SaveChangesAsync();

		return ServiceResult<bool>.Ok();
	}

	/// <inheritdoc />
	public async Task<ServiceResult<bool>> RemoveUserFromRole(UserUpdateRolesDto data)
	{
		var user = await GetUserWithRoles(data.UserId);
		var roleToRemove = user.Roles.FirstOrDefault(r => r.Id == data.RoleId);
		if (roleToRemove is null)
		{
			return ServiceResult<bool>.Unprocessable(ErrorMessages.Account.AccountNotInRole);
		}

		user.Roles.Remove(roleToRemove);
		await Context.SaveChangesAsync();

		return ServiceResult<bool>.Ok();
	}

	/// <inheritdoc />
	protected override async Task ValidateState(SienarUserDto model)
	{
		var user = await EntitySet.FirstOrDefaultAsync(
			u => u.Id != model.Id && u.Username == model.Username);
		if (user is not null)
		{
			throw new SienarConflictException(
				ErrorMessages.Account.UsernameTaken);
		}

		if (!string.IsNullOrEmpty(model.PendingEmail))
		{
			user = await EntitySet.FirstOrDefaultAsync(
				u => u.Id != model.Id
				&& (u.Email == model.Email
				|| u.Email == model.PendingEmail
				|| u.PendingEmail == model.Email
				|| u.PendingEmail == model.PendingEmail));
		}
		else
		{
			user = await EntitySet.FirstOrDefaultAsync(
				u => u.Id != model.Id
				&& (u.Email == model.Email
				|| u.PendingEmail == model.Email));
		}

		if (user is not null)
		{
			throw new SienarConflictException(
				ErrorMessages.Account.EmailTaken);
		}
	}

	/// <summary>
	/// Updates the <see cref="SienarUserDto.Password"/> if it has been modified 
	/// </summary>
	/// <param name="model">The user DTO to update</param>
	protected virtual void UpdatePasswordIfAppropriate(SienarUserDto model)
	{
		if (model.Password != LoginOptions.PasswordPlaceholderString)
		{
			model.Password = PasswordHasher.HashPassword(
				new(), // Seems sus, but the user parameter is unused for whatever reason
				model.Password);
		}
	}

	protected virtual async Task<TUser> GetUserWithRoles(Guid id)
	{
		var user = await EntitySet
			.Include(u => u.Roles)
			.FirstOrDefaultAsync(u => u.Id == id);

		if (user is null)
		{
			throw new SienarNotFoundException(ErrorMessages.Account.NotFound);
		}

		return user;
	}
}