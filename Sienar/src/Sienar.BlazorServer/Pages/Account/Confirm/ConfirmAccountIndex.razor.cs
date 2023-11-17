using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sienar.BlazorServer.Components;
using Sienar.Identity;
using Sienar.BlazorServer.Components.Pages;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Account.Confirm;

[AllowAnonymous]
[Route(Urls.Account.Confirm.Index)]
[Layout(typeof(StandaloneNarrowLayout))]
public partial class ConfirmAccountIndex<TUserDto>
	: ActionPage<ConfirmAccountIndex<TUserDto>, IAccountService, ConfirmAccountDto>
{
	[Parameter]
	[SupplyParameterFromQuery]
	public Guid UserId { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public Guid Code { get; set; }

	protected override async Task OnParametersSetAsync()
	{
		Model.UserId = UserId;
		Model.VerificationCode = Code;
		await SubmitRequest(() => Service.ConfirmAccount(Model));

		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Account.Confirm.Successful);
		}
	}
}