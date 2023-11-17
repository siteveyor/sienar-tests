using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sienar.Identity;

public interface IUserPersonalDataRetriever<TUser>
	where TUser : SienarUser
{
	/// <summary>
	/// Retrieves personal data 
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
	Task<Dictionary<string, string>> GetUserData(TUser user);
}