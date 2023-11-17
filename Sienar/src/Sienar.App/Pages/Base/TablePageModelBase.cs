using Microsoft.AspNetCore.Mvc;
using Sienar.Infrastructure;

namespace Sienar.Pages.Base;

public class TablePageModelBase<T> : PageModelBase<ICrudService<T>>
{
	public Filter Filter { get; private set; } = default!;

	public TablePageModelBase(
		ICrudService<T> service,
		IToastService toastService)
		: base(service, toastService) {}

	public IActionResult OnGet([FromQuery] Filter? filter)
	{
		Filter = filter ?? new();
		return Page();
	}
}