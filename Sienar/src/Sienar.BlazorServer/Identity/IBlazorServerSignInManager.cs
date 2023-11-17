using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Sienar.Identity;

public interface IBlazorServerSignInManager<TUser> : ISignInManager<TUser>
{
	/// <summary>
	/// Loads the current user's login status into the <see cref="AuthStateProvider"/>
	/// </summary>
	Task LoadUserLoginStatus();

	/// <summary>
	/// Refreshes the current user's login status as stored in the browser storage
	/// </summary>
	Task RefreshUserLoginStatus();
}