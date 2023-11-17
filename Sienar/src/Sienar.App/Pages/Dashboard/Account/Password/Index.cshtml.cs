using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Dashboard.Account.Password;

public class IndexModel : ActionPageModelWithGetBase<IAccountService>
{
	/// <inheritdoc />
	public IndexModel(
		IAccountService service,
		IToastService toastService)
		: base(service, toastService)
	{
		SuccessUrl = Urls.Dashboard.Account.PasswordChange.Successful;
	}

	[BindProperty]
	public ChangePasswordDto Dto { get; set; } = new ();

	/// <inheritdoc />
	protected override Task<ServiceResult<bool>> DoAction()
		=> Service.ChangePassword(Dto);
}