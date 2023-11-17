using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Dashboard.MyAccount.EmailChange;

[Route(Urls.Dashboard.MyAccount.EmailChange.Confirm)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class EmailChangeConfirm
{
	[Parameter]
	[SupplyParameterFromQuery]
	public Guid UserId { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public Guid Code { get; set; }

	[CascadingParameter]
	private Task<AuthenticationState> AuthState { get; set; } = default!;

	protected override async Task OnInitializedAsync()
	{
		ErrorMessage = "We are verifying your new email address. Please wait...";

		Model.UserId = UserId;
		Model.VerificationCode = Code;

		var authState = await AuthState;
		await SubmitRequest(() => Service.PerformEmailChange(Model, authState.User));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Dashboard.MyAccount.EmailChange.Successful);
		}
	}
}