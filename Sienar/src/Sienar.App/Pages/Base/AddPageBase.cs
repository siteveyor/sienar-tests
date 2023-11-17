using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Infrastructure;

namespace Sienar.Pages.Base;

public class AddPageBase<T> : ActionPageModelWithGetBase<ICrudService<T>>
	where T : new()
{
	/// <inheritdoc />
	public AddPageBase(
		ICrudService<T> service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = "Index";
	}

	[BindProperty]
	public T Dto { get; set; } = new();

	protected override Task<ServiceResult<bool>> DoAction()
		=> ConvertAddServiceCall(() => Service.Add(Dto));
}