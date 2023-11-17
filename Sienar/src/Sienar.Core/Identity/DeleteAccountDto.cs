using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sienar.Identity;

public class DeleteAccountDto
{
	[Required]
	[DisplayName("Password")]
	[DataType(DataType.Password)]
	public string Password { get; set; } = default!;
}