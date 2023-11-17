using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Components.Modals.MyAccount;

public partial class DeleteAccountConfirmation
{
	[CascadingParameter]
	private MudDialogInstance Instance { get; set; } = default!;

	[CascadingParameter]
	private Task<AuthenticationState> AuthState { get; set; } = default!;

	private async Task HandleSubmit()
	{
		var authState = await AuthState;
		// await SubmitRequest(() => Service.DeleteAccount(authState.User));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Account.Deleted);
			Close();
		}
	}

	private void Close()
	{
		Instance.Close();
	}
}