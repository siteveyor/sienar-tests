using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Dashboard.Account;

public class PersonalDataModel : PageModelBase<IAccountService>
{
	[BindProperty]
	public DeleteAccountDto Dto { get; set; } = new();

	/// <inheritdoc />
	public PersonalDataModel(
		IAccountService service,
		IToastService toastService)
		: base(service, toastService) {}

	public IActionResult OnGet() => Page();

	public Task<IActionResult> OnGetPersonalData()
		=> Execute(() => Service.GetPersonalData());

	public Task<IActionResult> OnPostDeleteAccount()
		=> Execute(() => Service.DeleteAccount(Dto), Urls.Account.Deleted);
}