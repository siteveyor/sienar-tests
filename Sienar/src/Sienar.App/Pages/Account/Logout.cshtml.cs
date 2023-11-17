using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Account;

public class LogoutModel : ActionPageModelBase<IAccountService>
{
	/// <inheritdoc />
	public LogoutModel(
		IAccountService service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = Urls.Home;
	}

	public Task<IActionResult> OnGet()
		=> Execute();

	/// <inheritdoc />
	protected override Task<ServiceResult<bool>> DoAction()
		=> Service.Logout();
}