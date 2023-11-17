using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Account;

[AllowAnonymous]
public class LoginModel : ActionPageModelWithGetBase<IAccountService>
{
	public LoginModel(
		IAccountService service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = Urls.Home;
	}

	[BindProperty]
	public LoginDto Account { get; set; } = new();

	protected override Task<ServiceResult<bool>> DoAction()
		=> Service.Login(Account);
}