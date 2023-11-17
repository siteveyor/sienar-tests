using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Sienar.Identity;

public class CookieSignInManager<TUser> : ISignInManager<TUser>
	where TUser : SienarUser
{
	protected readonly HttpContext HttpContext;
	protected readonly LoginOptions LoginOptions;
	protected readonly IUserClaimsPrincipalFactory<TUser> PrincipalFactory;

	public CookieSignInManager(
		IHttpContextAccessor contextAccessor,
		IOptions<LoginOptions> loginOptions,
		IUserClaimsPrincipalFactory<TUser> principalFactory)
	{
		HttpContext = contextAccessor.HttpContext 
			?? throw new ArgumentNullException(
				nameof(contextAccessor.HttpContext),
				"HttpContext cannot be null");
		LoginOptions = loginOptions.Value;
		PrincipalFactory = principalFactory;
	}

	/// <inheritdoc />
	public virtual async Task SignIn(TUser user, bool isPersistent)
	{
		var authProperties = new AuthenticationProperties
		{
			IsPersistent = isPersistent,
			AllowRefresh = true,
			IssuedUtc = DateTimeOffset.Now,
			ExpiresUtc = GetExpiration(isPersistent)
		};
		var claimsPrincipal = await PrincipalFactory.CreateAsync(user);
		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			claimsPrincipal,
			authProperties);
	}

	public virtual async Task SignOut()
	{
		await HttpContext.SignOutAsync(
			CookieAuthenticationDefaults.AuthenticationScheme);
	}

	protected DateTimeOffset GetExpiration(bool isPersistent)
	{
		var duration = isPersistent
			? TimeSpan.FromDays(LoginOptions.PersistentLoginDuration)
			: TimeSpan.FromHours(LoginOptions.TransientLoginDuration);

		return DateTimeOffset.Now + duration;
	}
}