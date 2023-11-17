using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Sienar.Infrastructure;
using Sienar.Tools;

namespace Sienar.RazorComponents;

public class TableHeadingBase : ComponentBase
{
	[Parameter]
	public string Name { get; set; } = default!;

	[Parameter]
	public bool NonSortable { get; set; }

	[Parameter]
	public RenderFragment ChildContent { get; set; } = default!;

	[CascadingParameter(Name = "Filter")]
	public Filter Filter { get; set; } = default!;

	[Inject]
	private IHttpContextAccessor ContextAccessor { get; set; } = default!;

	protected bool IsActive => Filter.SortName == Name;
	protected bool IsDescending => IsActive && (Filter.SortDescending ?? false);

	protected string CreateSortLink()
	{
		var sortDescendingFalse = Filter.SortDescending.HasValue && !Filter.SortDescending.Value;
		bool? newSortDescending = null;

		if (Filter.SortName != Name)
		{
			newSortDescending = false;
		}
		else if (sortDescendingFalse)
		{
			newSortDescending = true;
		}

		return LinkUtils.GenerateTableFilterLink(
			ContextAccessor.GetRequestPath(),
			Filter.SearchTerm,
			newSortDescending.HasValue ? Name : null,
			newSortDescending,
			Filter.Page,
			Filter.PageSize);
	}
}