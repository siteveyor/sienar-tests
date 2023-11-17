using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Account;

[AllowAnonymous]
[Route(Urls.Account.Login)]
[Layout(typeof(StandaloneNarrowLayout))]
public partial class Login
{
	[Parameter]
	public string? ReturnUri { get; set; }

	protected override async Task OnSubmit()
	{
		Logger.LogInformation("Submitting...");
		SetFormCompletionTime(Model);
	
		await SubmitRequest(() => Service.Login(Model));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(ReturnUri ?? Urls.Dashboard.Index);
		}
	}
}