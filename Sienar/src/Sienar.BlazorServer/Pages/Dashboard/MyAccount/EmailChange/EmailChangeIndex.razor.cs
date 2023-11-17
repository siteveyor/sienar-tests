using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Dashboard.MyAccount.EmailChange;

[Route(Urls.Dashboard.MyAccount.EmailChange.Index)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class EmailChangeIndex
{
	[CascadingParameter]
	private Task<AuthenticationState> AuthState { get; set; } = default!;

	protected override async Task OnSubmit()
	{
		var authState = await AuthState;
		await SubmitRequest(() => Service.InitiateEmailChange(Model, authState.User));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Dashboard.MyAccount.EmailChange.Requested);
		}
	}
}