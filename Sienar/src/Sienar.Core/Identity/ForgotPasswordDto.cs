#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sienar.Identity;

public class ForgotPasswordDto : HoneypotDto
{
	[Required]
	[DisplayName("Username or email")]
	public string AccountName { get; set; }
}