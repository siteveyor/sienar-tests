using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Account.ForgotPassword;

[AllowAnonymous]
public class IndexModel : ActionPageModelWithGetBase<IAccountService>
{
	/// <inheritdoc />
	public IndexModel(
		IAccountService service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = Urls.Account.ForgotPassword.Successful;
	}

	[BindProperty]
	public ForgotPasswordDto Forgot { get; set; } = new();

	/// <inheritdoc />
	protected override Task<ServiceResult<bool>> DoAction()
		=> Service.ForgotPassword(Forgot);
}