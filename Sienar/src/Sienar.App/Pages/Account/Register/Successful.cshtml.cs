using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sienar.Pages.Account.Register;

public class SuccessfulModel : PageModel
{
	public string? Username { get; private set; }
	public string? Email { get; private set; }

	public IActionResult OnGet(string? username, string? email)
	{
		Username = username;
		Email = email;
		return Page();
	}
}