using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;
using Sienar.Identity;

namespace Sienar.BlazorServer.Pages.Dashboard.Users;

[Authorize(Roles = Roles.Admin)]
[Route($"{Urls.Dashboard.Users.Index}/{{userId:guid}}")]
[Layout(typeof(DashboardNarrowLayout))]
public partial class ViewUserData
{
	[Parameter]
	public Guid UserId { get; set; }

	[Inject]
	private IDialogService DialogService { get; set; } = default!;

	private SienarUserDto? _user;

	protected override async Task OnInitializedAsync()
	{
		_user = await SubmitRequest(() => Service.Get(UserId));
	}
}