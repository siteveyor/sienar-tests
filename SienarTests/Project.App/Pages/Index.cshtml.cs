using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sienar.Infrastructure;

namespace Project.App.Pages;

public class IndexModel : PageModel
{
	private readonly IToastService _toastService;

	/// <inheritdoc />
	public IndexModel(IToastService toastService)
	{
		_toastService = toastService;
	}

	public IActionResult OnGet()
	{
		// var types = new[]
		// {
		// 	ToastType.Default,
		// 	ToastType.Success,
		// 	ToastType.Error,
		// 	ToastType.Warning,
		// 	ToastType.Info,
		// 	ToastType.None
		// };
		// foreach (var t in types)
		// {
		// 	_toastService.EnqueueToast(
		// 		new()
		// 		{
		// 			BodyText = $"Hello, {t.ToString()} toast!",
		// 			IsBackgroundTheme = true,
		// 			Type = t,
		// 			TitleText = "Just a toast!"
		// 		});
		// }

		return Page();
	}
}