using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Infrastructure;

namespace Sienar.Pages.Base;

public class EditPageBase<T> : ActionPageModelBase<ICrudService<T>>
{
	/// <inheritdoc />
	public EditPageBase(
		ICrudService<T> service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = "Index";
	}

	[BindProperty(SupportsGet = true)]
	public Guid Id { get; set; }

	[BindProperty]
	public T? Dto { get; set; }

	public async Task<IActionResult> OnGet()
	{
		Dto = (await Service.Get(Id)).Result;
		return Page();
	}

	/// <inheritdoc />
	protected override Task<ServiceResult<bool>> DoAction()
		=> Service.Edit(Dto!);
}