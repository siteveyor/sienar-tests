using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Account.ForgotPassword;

[AllowAnonymous]
[Route(Urls.Account.ForgotPassword.Index)]
[Layout(typeof(StandaloneNarrowLayout))]
public partial class ForgotPasswordIndex
{
	protected override async Task OnSubmit()
	{
		SetFormCompletionTime(Model);

		await SubmitRequest(() => Service.ForgotPassword(Model));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Account.ForgotPassword.Successful);
		}
	}
}