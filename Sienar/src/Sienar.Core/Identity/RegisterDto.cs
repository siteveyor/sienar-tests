#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Sienar.Validation;

namespace Sienar.Identity;

public class RegisterDto : HoneypotDto
{
	[Required]
	[DisplayName("Email")]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[DisplayName("Username")]
	[StringLength(32, ErrorMessage = "Username must be between 6 and 32 characters", MinimumLength = 6)]
	public string Username { get; set; }

	[Required]
	[DisplayName("Password")]
	[DataType(DataType.Password)]
	[StringLength(64, ErrorMessage = "Password must be between 8 and 64 characters", MinimumLength = 8)]
	[RequireUpper(ErrorMessage = "Your password must contain an uppercase letter")]
	[RequireLower(ErrorMessage = "Your password must contain a lowercase letter")]
	[RequireDigit(ErrorMessage = "Your password must contain a number")]
	[RequireSpecial(ErrorMessage = "Your password must contain a special character")]
	public string Password { get; set; }

	[Required]
	[DisplayName("Confirm password")]
	[DataType(DataType.Password)]
	[Compare("Password", ErrorMessage = "The passwords do not match")]
	public string ConfirmPassword { get; set; }

	public bool AcceptTos { get; set; }
}