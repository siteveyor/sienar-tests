using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sienar.BlazorServer.Infrastructure;

namespace Sienar.BlazorServer.Components.UI;

public partial class AppTheme
{
	[Inject]
	private MudTheme Theme { get; set; } = default!;
}