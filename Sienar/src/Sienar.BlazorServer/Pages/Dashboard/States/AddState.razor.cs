using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sienar.BlazorServer.Components;
using Sienar.BlazorServer.Tools;

namespace Sienar.BlazorServer.Pages.Dashboard.States;

[Authorize(Roles = Roles.Admin)]
[Route(Urls.Dashboard.States.Add)]
[Layout(typeof(DashboardNarrowLayout))]
public partial class AddState
{
	protected override async Task OnSubmit()
	{
		await SubmitRequest(() => Service.Add(Model));
		if (WasSuccessful)
		{
			Reset();
		}
	}
}