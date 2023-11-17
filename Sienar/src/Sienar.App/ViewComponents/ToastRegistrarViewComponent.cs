using Microsoft.AspNetCore.Mvc;
using Sienar.Infrastructure;

namespace Sienar.ViewComponents;

public class ToastRegistrarViewComponent : ViewComponent
{
	private readonly IToastService _toastService;

	/// <inheritdoc />
	public ToastRegistrarViewComponent(IToastService toastService)
	{
		_toastService = toastService;
	}

	public IViewComponentResult Invoke()
		=> View(_toastService.GetToasts());
}