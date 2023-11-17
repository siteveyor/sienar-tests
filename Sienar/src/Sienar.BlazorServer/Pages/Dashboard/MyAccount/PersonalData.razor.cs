using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MudBlazor;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Components.Modals.MyAccount;
using Sienar.BlazorServer.Tools;
using Sienar.Identity;

namespace Sienar.BlazorServer.Pages.Dashboard.MyAccount;

[Route(Urls.Dashboard.MyAccount.PersonalData)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class PersonalData
{
	[Inject]
	private IAccountService Service { get; set; } = default!;

	[Inject]
	private ILogger<PersonalData> Logger { get; set; } = default!;

	[Inject]
	private IJSRuntime Interop { get; set; } = default!;

	[Inject]
	private IDialogService Dialog { get; set; } = default!;

	[CascadingParameter]
	private Task<AuthenticationState> AuthState { get; set; } = default!;

	private bool _isLoading;

	private async Task DownloadPersonalData()
	{
		if (_isLoading)
		{
			return;
		}

		Logger.LogInformation("Downloading personal data");

		_isLoading = true;
		var data = await Service.GetPersonalData((await AuthState).User);
		if (!data.WasSuccessful)
		{
			return;
		}

		using var stream = new MemoryStream(data.Result!.Contents);
		using var dataWrapper = new DotNetStreamReference(stream);
		await Interop.InvokeVoidAsync("downloadFileFromStream", "PersonalData.json", dataWrapper);
		_isLoading = false;
	}

	private void LaunchDeleteAccountDialog()
	{
		Dialog.Show<DeleteAccountConfirmation>();
	}
}