using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Sienar.Identity;

public class UserClaimsPrincipalFactory<TUser>
	: IUserClaimsPrincipalFactory<TUser>
	where TUser : SienarUser
{
	protected readonly IUserClaimsFactory<TUser> ClaimsFactory;

	public UserClaimsPrincipalFactory(IUserClaimsFactory<TUser> claimsFactory)
	{
		ClaimsFactory = claimsFactory;
	}

	public virtual async Task<ClaimsPrincipal> CreateAsync(TUser user)
	{
		var identity = await GenerateClaims(user);
		return new ClaimsPrincipal(identity);
	}

	protected virtual Task<ClaimsIdentity> GenerateClaims(TUser user)
	{
		var identity = new ClaimsIdentity(
			ClaimsFactory.CreateClaims(user),
			CookieAuthenticationDefaults.AuthenticationScheme);
		return Task.FromResult(identity);
	}
}