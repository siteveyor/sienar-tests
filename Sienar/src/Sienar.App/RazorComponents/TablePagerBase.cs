using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Sienar.Infrastructure;
using Sienar.Tools;

namespace Sienar.RazorComponents;

public class TablePagerBase<TDto> : ComponentBase
{
	[Parameter]
	public Filter Filter { get; set; } = default!;

	[Parameter]
	public PagedDto<TDto> Items { get; set; } = default!;

	[Inject]
	private IHttpContextAccessor ContextAccessor { get; set; } = default!;

	protected int TotalPages => (int)Math.Ceiling((double)Items.TotalCount / Filter.PageSize);

	protected bool IsFirstPage => Filter.Page == 1;
	protected bool IsLastPage => Filter.Page == TotalPages;
	protected int StartIndex => Filter.PageSize * (Filter.Page - 1) + 1;
	protected int EndIndex
	{
		get
		{
			var product = Filter.PageSize * Filter.Page;
			return product > Items.TotalCount ? Items.TotalCount : product;
		}
	}

	protected IEnumerable<int> PageSizeNumbers => new[] { 5, 10, 25 };

	protected IEnumerable<int> PaginationPageNumbers => new []
	{
		Filter.Page - 2,
		Filter.Page - 1,
		Filter.Page,
		Filter.Page + 1,
		Filter.Page + 2
	};

	protected string CreatePageSizeUrl(int pageSize)
	{
		return LinkUtils.GenerateTableFilterLink(
			ContextAccessor.GetRequestPath(),
			Filter.SearchTerm,
			Filter.SortName,
			Filter.SortDescending,
			Filter.Page,
			pageSize);
	}

	protected string CreatePageUrl(int page)
	{
		return LinkUtils.GenerateTableFilterLink(
			ContextAccessor.GetRequestPath(),
			Filter.SearchTerm,
			Filter.SortName,
			Filter.SortDescending,
			page,
			Filter.PageSize);
	}
}