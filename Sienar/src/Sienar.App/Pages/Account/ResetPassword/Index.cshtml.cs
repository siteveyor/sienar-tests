using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Account.ResetPassword;

[AllowAnonymous]
public class IndexModel : ActionPageModelBase<IAccountService>
{
	/// <inheritdoc />
	public IndexModel(
		IAccountService service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = Urls.Account.ResetPassword.Successful;
	}

	[BindProperty]
	public ResetPasswordDto ResetPassword { get; set; } = new();

	public IActionResult OnGet(Guid userId, Guid code)
	{
		ResetPassword.UserId = userId;
		ResetPassword.VerificationCode = code;
		return Page();
	}

	/// <inheritdoc />
	protected override Task<ServiceResult<bool>> DoAction()
		=> Service.ResetPassword(ResetPassword);
}