using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Account.Register;

[AllowAnonymous]
[Route(Urls.Account.Register.Index)]
[Layout(typeof(StandaloneNarrowLayout))]
public partial class RegisterIndex
{
	protected override async Task OnSubmit()
	{
		// MudBlazor doesn't show errors on checkboxes
		// so unfortunately, a [RequireTrue] wouldn't do any good
		if (!Model.AcceptTos)
		{
			ErrorMessage = "You must accept the Terms of Service and Privacy Policy to register.";
			return;
		}

		SetFormCompletionTime(Model);

		await SubmitRequest(() => Service.Register(Model));

		if (WasSuccessful)
		{
			NavManager.NavigateTo($"{Urls.Account.Register.Successful}?username={Model.Username}&email={Model.Email}");
		}
	}
}