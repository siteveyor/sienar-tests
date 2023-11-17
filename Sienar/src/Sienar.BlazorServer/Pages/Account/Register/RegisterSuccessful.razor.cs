using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Account.Register;

[AllowAnonymous]
[Route(Urls.Account.Register.Successful)]
[Layout(typeof(StandaloneNarrowLayout))]
public partial class RegisterSuccessful
{
	[Parameter]
	[SupplyParameterFromQuery]
	public string Username { get; set; } = default!;

	[Parameter]
	[SupplyParameterFromQuery]
	public string Email { get; set; } = default!;
}