using Microsoft.AspNetCore.Components;
using Sienar.Infrastructure;

namespace Sienar.RazorComponents;

public class TableFormBase : ComponentBase
{
	[Parameter]
	public Filter Filter { get; set; } = default!;
}