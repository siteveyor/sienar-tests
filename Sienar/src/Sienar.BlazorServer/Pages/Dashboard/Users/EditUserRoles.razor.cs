using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sienar.BlazorServer.Components;
using Sienar.Identity;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Dashboard.Users;

[Authorize(Roles = Roles.Admin)]
[Route($"{Urls.Dashboard.Users.Index}/{{userId:guid}}/roles")]
[Layout(typeof(DashboardNarrowLayout))]
public partial class EditUserRoles
{
	[Parameter]
	public Guid UserId { get; set; }

	// private CheckboxAggregator _aggregator = default!;

	private SienarUserDto? _user;

	protected override async Task OnInitializedAsync()
	{
		_user = await SubmitRequest(() => Service.Get(UserId));
		Model.UserId = UserId;
	}

	protected override async Task OnSubmit()
	{
		// TODO: set up proper role management
		// Model.Roles = _aggregator.GetSelected();
		// await SubmitRequest(() => Service.AddUserToRole(Model));
	}
}