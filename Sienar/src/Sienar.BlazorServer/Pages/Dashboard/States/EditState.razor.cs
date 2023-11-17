using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;
using Sienar.Infrastructure.States;

namespace Sienar.BlazorServer.Pages.Dashboard.States;

[Authorize(Roles = Roles.Admin)]
[Route($"{Urls.Dashboard.States.Edit}/{{id:guid}}")]
[Layout(typeof(DashboardNarrowLayout))]
public partial class EditState
{
	[Parameter]
	public Guid StateId { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Model = await SubmitRequest(() => Service.Get(StateId)) ?? new StateDto();
		Model.Id = StateId;
	}

	protected override async Task OnSubmit()
	{
		await SubmitRequest(() => Service.Edit(Model));
		if (WasSuccessful)
		{
			NavManager.NavigateTo(Urls.Dashboard.States.Index);
		}
	}
}