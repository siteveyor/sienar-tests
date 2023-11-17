using Microsoft.AspNetCore.Components;

namespace Sienar.BlazorServer.Components.UI;

public partial class SharedLayout
{
	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;
}