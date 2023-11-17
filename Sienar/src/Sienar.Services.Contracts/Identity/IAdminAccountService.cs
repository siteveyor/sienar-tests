using System.Threading.Tasks;
using Sienar.Infrastructure;

namespace Sienar.Identity;

public interface IAdminAccountService
	: ICrudService<SienarUserDto>
{
	/// <summary>
	/// Adds the specified user to the specified role
	/// </summary>
	/// <param name="dto">The <see cref="UserUpdateRolesDto"/> containing user data</param>
	/// <returns>whether the operation was successful</returns>
	Task<ServiceResult<bool>> AddUserToRole(UserUpdateRolesDto dto);

	/// <summary>
	/// Removes the specified user from the specified role
	/// </summary>
	/// <param name="dto">The <see cref="UserUpdateRolesDto"/> containing user data</param>
	/// <returns>whether the operation was successful</returns>
	Task<ServiceResult<bool>> RemoveUserFromRole(UserUpdateRolesDto dto);
}