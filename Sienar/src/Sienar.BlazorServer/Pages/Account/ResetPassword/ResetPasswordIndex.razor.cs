using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Account.ResetPassword;

[AllowAnonymous]
[Route(Urls.Account.ResetPassword.Index)]
[Layout(typeof(StandaloneNarrowLayout))]
public partial class ResetPasswordIndex
{
	[Parameter]
	[SupplyParameterFromQuery]
	public Guid UserId { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public Guid Code { get; set; }

	protected override void OnInitialized()
	{
		Model.UserId = UserId;
		Model.VerificationCode = Code;
	}

	protected override async Task OnSubmit()
	{
		SetFormCompletionTime(Model);

		await SubmitRequest(() => Service.ResetPassword(Model));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Account.ResetPassword.Successful);
		}
	}
}