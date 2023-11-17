#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sienar.Identity;

public class LoginDto : HoneypotDto
{
	[Required]
	[DisplayName("Username or email")]
	public string AccountName { get; set; }

	[Required]
	[DisplayName("Password")]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	[DisplayName("Remember me")]
	public bool RememberMe { get; set; }
}