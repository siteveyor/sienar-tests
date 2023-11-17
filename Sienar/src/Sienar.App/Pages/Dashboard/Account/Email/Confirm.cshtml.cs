using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Dashboard.Account.Email;

public class ConfirmModel : ActionPageModelBase<IAccountService>
{
	private PerformEmailChangeDto _dto = new();

	/// <inheritdoc />
	public ConfirmModel(
		IAccountService service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = Urls.Dashboard.Account.EmailChange.Successful;
	}

	public Task<IActionResult> OnGet(Guid userId, Guid code)
	{
		_dto.UserId = userId;
		_dto.VerificationCode = code;
		return Execute();
	}

	/// <inheritdoc />
	protected override Task<ServiceResult<bool>> DoAction()
		=> Service.PerformEmailChange(_dto);
}