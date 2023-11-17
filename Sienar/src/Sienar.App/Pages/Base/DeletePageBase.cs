using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Infrastructure;

namespace Sienar.Pages.Base;

public class DeletePageBase<T> : ActionPageModelBase<ICrudService<T>>
{
	public DeletePageBase(
		ICrudService<T> service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = "Index";
	}

	[BindProperty(SupportsGet = true)]
	public Guid Id { get; set; }

	public T? Dto { get; set; }

	public async Task<IActionResult> OnGet()
	{
		Dto = (await Service.Get(Id)).Result;
		return Page();
	}

	/// <inheritdoc />
	protected override Task<ServiceResult<bool>> DoAction()
		=> Service.Delete(Id);
}