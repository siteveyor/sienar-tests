using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;
using Sienar.Identity;

namespace Sienar.BlazorServer.Pages.Account;

[AllowAnonymous]
[Route(Urls.Account.Logout)]
[Layout(typeof(StandaloneNarrowLayout))]
public partial class Logout
{
	[Inject]
	private IAccountService AccountService { get; set; } = default!;

	[Inject]
	private NavigationManager NavManager { get; set; } = default!;

	private string _message = "Logging you out. Please wait...";

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			_ = DoLogout();
		}
	}

	// This is extracted into a separate method because if the logic is awaited, one of two things happens: 1) if you don't manually call StateHasChanged(), the logout page never updates because it only renders once, or 2) if you do manually call StateHasChanged(), the rest of the UI seems to not respond to the logout event until you reload the page. By separating the logic into a separate method that is async but not awaited, 
	private async Task DoLogout()
	{
		var result = await AccountService.Logout(); 
		if (result.WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Home);
		}
		else
		{
			_message = "We were unable to log you out.";
		}
	}
}