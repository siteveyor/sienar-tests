using Microsoft.AspNetCore.Components;
using Sienar.Infrastructure;

namespace Sienar.RazorComponents;

public class TableBase<TDto> : ComponentBase
{
	[Parameter]
	public string Title { get; set; } = default!;

	[Parameter]
	public PagedDto<TDto> Items { get; set; } = new();

	[Parameter]
	public Filter Filter { get; set; } = default!;

	[Parameter]
	public RenderFragment TableHead { get; set; } = default!;

	[Parameter]
	public RenderFragment<TDto> TableBody { get; set; } = default!;
}