using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sienar.Infrastructure;
using Sienar.Tools;

namespace Sienar.Pages.Base;

public class PageModelBase<TService> : PageModel
{
	protected readonly TService Service;
	protected readonly IToastService ToastService;

	/// <inheritdoc />
	protected PageModelBase(
		TService service,
		IToastService toastService)
	{
		Service = service;
		ToastService = toastService;
	}

	public string? ErrorMessage { get; protected set; }
	public string SuccessUrl { get; set; } = string.Empty;
	protected object? RouteValues { get; set; }

	/// <summary>
	/// Executes an arbitrary action and redirects to <c>successPath</c> on success
	/// </summary>
	/// <param name="callback">The action to take</param>
	/// <param name="successPath">The URL to redirect to on success</param>
	/// <returns></returns>
	protected async Task<IActionResult> Execute(
		Func<Task<ServiceResult<bool>>> callback,
		string? successPath)
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		var result = await callback();

		if (!string.IsNullOrEmpty(result.Message))
		{
			ToastService.EnqueueToast(result.MessageType, result.Message);
		}

		if (result.WasSuccessful)
		{
			var returnUrl = Request.GetReturnUrl();
			if (!string.IsNullOrEmpty(returnUrl))
			{
				// No need to pass route values if a ReturnUrl was provided because the ReturnUrl includes any query string values from the original request URI that redirected to the login page
				return LocalRedirect(returnUrl);
			}

			return RedirectToPage(successPath, RouteValues);
		}

		ErrorMessage = result.Message;
		return Page();
	}

	/// <summary>
	/// Executes an arbitrary action and returns a file on success
	/// </summary>
	/// <param name="callback">The action to take</param>
	/// <returns></returns>
	protected async Task<IActionResult> Execute(Func<Task<ServiceResult<FileDto>>> callback)
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		var result = await callback();
		if (result.WasSuccessful)
		{
			var file = result.Result!;

			return File(file.Contents, file.Mime, file.Name);
		}

		ErrorMessage = result.Message;
		return Page();
	}

	protected async Task<ServiceResult<bool>> ConvertAddServiceCall(Func<Task<ServiceResult<Guid>>> adder)
	{
		var result = await adder();
		var newResult = ServiceResult<bool>.Ok();
		newResult.Message = result.Message;
		newResult.ServiceError = result.ServiceError;
		return newResult;
	}
}