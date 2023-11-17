using System.Collections.Generic;
using System.Security.Claims;

namespace Sienar.Identity;

public interface IUserClaimsFactory<TUser> where TUser : SienarUser
{
	IEnumerable<Claim> CreateClaims(TUser user);
}