using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Infrastructure;

namespace Sienar.Pages.Base;

public abstract class ActionPageModelBase<TService> : PageModelBase<TService>
{
	/// <inheritdoc />
	protected ActionPageModelBase(
		TService service,
		IToastService toastService)
		: base(service, toastService) {}

	/// <summary>
	/// The base implementation of POST request handling, which returns <see cref="Execute()"/>. <see cref="Execute()"/> calls <see cref="DoAction"/> and redirects to <see cref="PageModelBase{TService}.SuccessUrl"/> if the callback returns true.
	/// </summary>
	/// <returns></returns>
	public virtual Task<IActionResult> OnPost() => Execute();

	/// <summary>
	/// Takes the default action specified by the page and redirects to <see cref="PageModelBase{TService}.SuccessUrl"/> on success
	/// </summary>
	/// <returns></returns>
	protected Task<IActionResult> Execute() => Execute(SuccessUrl);

	/// <summary>
	/// Takes the default action specified by the page and redirects to <c>successPath</c> on success
	/// </summary>
	/// <param name="successPath">The URL to redirect to on success</param>
	/// <returns></returns>
	protected Task<IActionResult> Execute(string? successPath)
		=> Execute(DoAction, successPath);

	/// <summary>
	/// Do whatever work you want your page to do. Return a <c>ServiceResult&lt;bool&gt;</c>
	/// </summary>
	/// <returns></returns>
	protected abstract Task<ServiceResult<bool>> DoAction();
}