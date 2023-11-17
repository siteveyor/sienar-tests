using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Account.Confirm;

[AllowAnonymous]
public class IndexModel : ActionPageModelBase<IAccountService>
{
	private ConfirmAccountDto _dto = new();

	/// <inheritdoc />
	public IndexModel(
		IAccountService service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = Urls.Account.Confirm.Successful;
	}

	public Task<IActionResult> OnGet(Guid userId, Guid code)
	{
		_dto.UserId = userId;
		_dto.VerificationCode = code;
		return Execute();
	}

	/// <inheritdoc />
	protected override Task<ServiceResult<bool>> DoAction()
		=> Service.ConfirmAccount(_dto);
}