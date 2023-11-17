using System;
using Microsoft.AspNetCore.Components;

namespace Sienar.BlazorServer.Components.UI;

public partial class DashboardSharedLayout
{
	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;
}