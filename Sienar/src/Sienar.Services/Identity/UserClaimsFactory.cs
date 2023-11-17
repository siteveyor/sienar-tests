using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Sienar.Identity;

public class UserClaimsFactory<TUser> : IUserClaimsFactory<TUser>
	where TUser : SienarUser
{
	/// <inheritdoc />
	public IEnumerable<Claim> CreateClaims(TUser user)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new(ClaimTypes.Name, user.Username),
			new(ClaimTypes.Email, user.Email)
		};

		if (user.Roles is not null)
		{
			claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));
		}

		return claims;
	}
}