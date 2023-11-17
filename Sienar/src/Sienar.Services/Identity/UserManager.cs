using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Sienar.Identity;

public class UserManager<TUser, TContext>
	: IUserManager<TUser>
	where TUser : SienarUser
	where TContext : DbContext
{
	protected readonly TContext Context;
	protected readonly IPasswordHasher<TUser> PasswordHasher;
	protected DbSet<TUser> UserSet => Context.Set<TUser>();

	public UserManager(
		TContext context,
		IPasswordHasher<TUser> passwordHasher)
	{
		Context = context;
		PasswordHasher = passwordHasher;
	}

	/// <inheritdoc />
	public virtual Task<TUser?> GetUser(ClaimsPrincipal claims)
		=> GetUser<bool>(claims, null);

	/// <inheritdoc />
	public virtual async Task<TUser?> GetUser<TProperty>(
		ClaimsPrincipal claims,
		Expression<Func<TUser, TProperty>>? include)
	{
		var id = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
		if (id is null)
		{
			return null;
		}

		var guidId = Guid.Parse(id.Value);
		return await GetUser(u => u.Id == guidId, include);
	}

	/// <inheritdoc />
	public virtual Task<TUser?> GetUser(Guid id)
		=> GetUser<bool>(id, null);

	/// <inheritdoc />
	public virtual Task<TUser?> GetUser<TProperty>(
		Guid id,
		Expression<Func<TUser, TProperty>>? include)
		=> GetUser(u => u.Id == id, include);

	/// <inheritdoc />
	public virtual Task<TUser?> GetUser(string name)
		=> GetUser<bool>(name, null);

	/// <inheritdoc />
	public virtual async Task<TUser?> GetUser<TProperty>(
		string name,
		Expression<Func<TUser, TProperty>>? include)
		=> await GetUser(
			u => u.Username == name || u.Email == name,
			include);

	/// <inheritdoc />
	public virtual async Task UpdatePassword(
		TUser user,
		string newPassword)
	{
		user.PasswordHash = PasswordHasher.HashPassword(user, newPassword);
		Context.Update(user);
		await Context.SaveChangesAsync();
	}

	/// <inheritdoc />
	public virtual async Task<bool> VerifyPassword(TUser user, string password)
	{
		var verification = PasswordHasher.VerifyHashedPassword(
			user,
			user.PasswordHash,
			password);

		if (verification == PasswordVerificationResult.Failed)
		{
			return false;
		}

		if (verification == PasswordVerificationResult.SuccessRehashNeeded)
		{
			user.PasswordHash = PasswordHasher.HashPassword(user, password);
			Context
				.Set<TUser>()
				.Update(user);
			await Context.SaveChangesAsync();
		}

		return true;
	}

	/// <inheritdoc />
	public virtual async Task DeleteUser(ClaimsPrincipal claims)
	{
		var user = await GetUser(claims);
		if (user is null)
		{
			return;
		}

		UserSet.Remove(user);
		await Context.SaveChangesAsync();
	}

	/// <summary>
	/// Gets a single user if it exists based on the given predicate. Optionally includes relations.
	/// </summary>
	/// <param name="where">The predicate to search for the single user by</param>
	/// <returns></returns>
	protected async Task<TUser?> GetUser(Expression<Func<TUser, bool>> where)
		=> await UserSet.FirstOrDefaultAsync(where);

	/// <summary>
	/// Gets a single user if it exists based on the given predicate. Optionally includes relations.
	/// </summary>
	/// <param name="where">The predicate to search for the single user by</param>
	/// <param name="include">An expression to pass to the <c>IQueryable&lt;TUser&gt;.Include()</c> method</param>
	/// <returns></returns>
	protected async Task<TUser?> GetUser<TProperty>(
		Expression<Func<TUser, bool>> where,
		Expression<Func<TUser, TProperty>>? include)
	{
		IQueryable<TUser> users = UserSet;
		if (include is not null)
		{
			users = users.Include(include);
		}
		return await users.FirstOrDefaultAsync(where);
	}
}