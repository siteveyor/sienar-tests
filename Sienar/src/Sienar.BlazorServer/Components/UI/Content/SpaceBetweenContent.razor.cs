using Microsoft.AspNetCore.Components;

namespace Sienar.BlazorServer.Components.UI.Content;

public partial class SpaceBetweenContent
{
	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;
}