using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Sienar.Identity;

public class BlazorServerSignInManager<TUser, TUserDto> : IBlazorServerSignInManager<TUser>
	where TUser : SienarUser
	where TUserDto : SienarUserDto
{
	protected const string LocalLoginDataKey = "Sienar.Login";

	protected readonly LoginOptions LoginOptions;
	protected readonly IUserManager<TUser> UserManager;
	protected readonly IUserClaimsFactory<TUser> ClaimsFactory;
	protected readonly AuthStateProvider AuthStateProvider;
	protected readonly AccountStateProvider AccountStateProvider;
	protected readonly IDtoAdapter<TUser, TUserDto> UserAdapter;
	protected readonly ProtectedLocalStorage LocalStore;

	public BlazorServerSignInManager(
		IOptions<LoginOptions> loginOptions,
		IUserManager<TUser> userManager,
		IUserClaimsFactory<TUser> claimsFactory,
		AuthStateProvider authStateProvider,
		AccountStateProvider accountStateProvider,
		IDtoAdapter<TUser, TUserDto> userAdapter,
		ProtectedLocalStorage localStore)
	{
		LoginOptions = loginOptions.Value;
		UserManager = userManager;
		ClaimsFactory = claimsFactory;
		AuthStateProvider = authStateProvider;
		AccountStateProvider = accountStateProvider;
		UserAdapter = userAdapter;
		LocalStore = localStore;
	}

	/// <inheritdoc />
	public virtual async Task SignIn(TUser user, bool isPersistent)
	{
		var loginData = CreateLoginData(user.Id, isPersistent, true);
		await LocalStore.SetAsync(LocalLoginDataKey, loginData);

		var claims = ClaimsFactory.CreateClaims(user);
		AuthStateProvider.NotifyUserAuthentication(claims, true);
		AccountStateProvider.User = UserAdapter.MapToDto(user);
	}

	public virtual async Task SignOut()
	{
		var loginData = CreateLoginData(Guid.Empty);
		await LocalStore.SetAsync(LocalLoginDataKey, loginData);
		AuthStateProvider.NotifyUserAuthentication(Array.Empty<Claim>(), false);
		AccountStateProvider.User = null;
	}

	/// <inheritdoc />
	public async Task LoadUserLoginStatus()
	{
		var dataRequest = await LocalStore.GetAsync<BlazorServerLoginData>(LocalLoginDataKey);
		if (!dataRequest.Success)
		{
			return;
		}

		var loginData = dataRequest.Value!;
		if (!LoginValid(loginData))
		{
			await LocalStore.SetAsync(LocalLoginDataKey, CreateLoginData(Guid.Empty));
			AccountStateProvider.User = null;
			return;
		}

		var user = await UserManager.GetUser(loginData.UserId);
		if (user is null)
		{
			await LocalStore.SetAsync(LocalLoginDataKey, CreateLoginData(Guid.Empty));
			AccountStateProvider.User = null;
			return;
		}

		var claims = ClaimsFactory.CreateClaims(user);
		AuthStateProvider.NotifyUserAuthentication(claims, loginData.IsAuthenticated);
		AccountStateProvider.User = UserAdapter.MapToDto(user);
	}

	/// <inheritdoc />
	public async Task RefreshUserLoginStatus()
	{
		var dataRequest = await LocalStore.GetAsync<BlazorServerLoginData>(LocalLoginDataKey);
		if (!dataRequest.Success)
		{
			return;
		}

		var loginData = dataRequest.Value!;
		if (!LoginValid(loginData))
		{
			return;
		}

		var newLoginData = CreateLoginData(
			loginData.UserId,
			loginData.IsPersistent,
			loginData.IsAuthenticated);
		await LocalStore.SetAsync(LocalLoginDataKey, newLoginData);
	}

	protected BlazorServerLoginData CreateLoginData(
		Guid userId,
		bool isPersistent = false,
		bool isAuthenticated = false)
	{
		return new BlazorServerLoginData
		{
			UserId = userId,
			ExpiresAt = GetExpiration(isPersistent),
			IsPersistent = isPersistent,
			IsAuthenticated = isAuthenticated
		};
	}

	protected DateTimeOffset GetExpiration(bool isPersistent)
	{
		var duration = isPersistent
			? TimeSpan.FromDays(LoginOptions.PersistentLoginDuration)
			: TimeSpan.FromHours(LoginOptions.TransientLoginDuration);

		return DateTimeOffset.Now + duration;
	}

	protected bool LoginValid(BlazorServerLoginData data)
		=> data.IsAuthenticated && data.ExpiresAt > DateTimeOffset.Now;
}