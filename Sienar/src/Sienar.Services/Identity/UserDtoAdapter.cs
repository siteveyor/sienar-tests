using System.Linq;
using Microsoft.Extensions.Options;

namespace Sienar.Identity;

public class UserDtoAdapter<TUser, TUserDto>
	: IDtoAdapter<TUser, TUserDto>
	where TUser : SienarUser
	where TUserDto : SienarUserDto, new()
{
	protected readonly LoginOptions LoginOptions;
	protected readonly IDtoAdapter<SienarRole, SienarRoleDto> RoleDtoAdapter;

	public UserDtoAdapter(
		IOptions<LoginOptions> loginOptions,
		IDtoAdapter<SienarRole, SienarRoleDto> roleDtoAdapter)
	{
		LoginOptions = loginOptions.Value;
		RoleDtoAdapter = roleDtoAdapter;
	}

	/// <inheritdoc />
	public void MapToEntity(TUserDto dto, TUser entity)
	{
		entity.Username = dto.Username;
		entity.Email = dto.Email;
		entity.PendingEmail = dto.PendingEmail;
		entity.PhoneNumber = dto.PhoneNumber;

		if (dto.Password != LoginOptions.PasswordPlaceholderString)
		{
			entity.PasswordHash = dto.Password;
		}
	}

	/// <inheritdoc />
	public TUserDto MapToDto(TUser user)
	{
		var dto = new TUserDto
		{
			Id = user.Id,
			Username = user.Username,
			Email = user.Email,
			PendingEmail = user.PendingEmail,
			PhoneNumber = user.PhoneNumber,
			Password = LoginOptions.PasswordPlaceholderString,
			EmailConfirmed = user.EmailConfirmed,
			PhoneConfirmed = user.PhoneNumberConfirmed,
			ConcurrencyStamp = user.ConcurrencyStamp
		};

		if (user.Roles?.Count > 0)
		{
			dto.Roles = user.Roles.Select(r => RoleDtoAdapter.MapToDto(r));
		}

		return dto;
	}
}