using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Infrastructure;

namespace Sienar.Pages.Base;

public abstract class ActionPageModelWithGetBase<TService> : ActionPageModelBase<TService>
{
	/// <inheritdoc />
	public ActionPageModelWithGetBase(
		TService service,
		IToastService toastService)
		: base(service, toastService) {}

	/// <summary>
	/// The base implementation of GET request handling, which merely returns a view
	/// </summary>
	/// <returns></returns>
	public virtual Task<IActionResult> OnGet()
		=> Task.FromResult((IActionResult) Page());
}