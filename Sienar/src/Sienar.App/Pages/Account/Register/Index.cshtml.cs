using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Account.Register;

[AllowAnonymous]
public class IndexModel : ActionPageModelWithGetBase<IAccountService>
{
	public IndexModel(
		IAccountService service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = Urls.Account.Register.Successful;
	}

	[BindProperty]
	public RegisterDto Register { get; set; } = new();

	protected override Task<ServiceResult<bool>> DoAction()
	{
		RouteValues = new
		{
			Register.Username,
			Register.Email
		};

		return Service.Register(Register);
	}
}