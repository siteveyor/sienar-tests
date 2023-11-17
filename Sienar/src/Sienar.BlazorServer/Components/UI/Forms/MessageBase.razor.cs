using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Sienar.BlazorServer.Components.UI.Forms;

public partial class MessageBase
{
	[Parameter]
	public string? Message { get; set; }

	[Parameter]
	public Color Color { get; set; }

	[Parameter]
	public Typo Typo { get; set; } = Typo.h6;
}