using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sienar.Infrastructure;

namespace Sienar.ViewComponents;

public class AdminBarViewComponent : ViewComponent
{
	private readonly IUserAccessor _userAccessor;
	private readonly HttpContext _httpContext;

	/// <inheritdoc />
	public AdminBarViewComponent(
		IUserAccessor userAccessor,
		IHttpContextAccessor httpContextAccessor)
	{
		_userAccessor = userAccessor;
		_httpContext = httpContextAccessor.HttpContext!;
	}

	public IViewComponentResult Invoke()
	{
		var data = new AdminBarViewModel
		{
			IsSignedIn = _userAccessor.IsSignedIn(),
			IsAdmin = _userAccessor.UserInRole(Roles.Admin),
			IsOnDashboard = _httpContext.Request.Path.StartsWithSegments("/Dashboard"),
			Username = _userAccessor.GetUsername()
		};

		return View(data);
	}
}