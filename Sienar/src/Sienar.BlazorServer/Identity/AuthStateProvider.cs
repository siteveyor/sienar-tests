using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

// ReSharper disable once CheckNamespace
namespace Sienar.Identity;

public class AuthStateProvider : AuthenticationStateProvider
{
	private AuthenticationState? _authState;

	public override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		_authState ??= CreateAuthStateFromClaims(Array.Empty<Claim>(), false);
		return Task.FromResult(_authState);
	}

	public void NotifyUserAuthentication(IEnumerable<Claim> claims, bool isAuthenticated)
	{
		_authState = CreateAuthStateFromClaims(claims, isAuthenticated);
		NotifyAuthenticationStateChanged(Task.FromResult(_authState));
	}

	private static AuthenticationState CreateAuthStateFromClaims(
		IEnumerable<Claim> claims,
		bool isAuthenticated)
	{
		var identity = isAuthenticated
			? new ClaimsIdentity(claims, "BlazorServerBrowserAuth")
			: new ClaimsIdentity(claims);

		var principal = new ClaimsPrincipal(identity);
		return new AuthenticationState(principal);
	}
}