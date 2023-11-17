#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sienar.Infrastructure.States;

public class StateDto : EntityBase
{
	[Required]
	[DisplayName("Name")]
	[MaxLength(20, ErrorMessage = "The state name must be no longer than 20 characters long")]
	public string Name { get; set; }

	[Required]
	[DisplayName("Abbreviation")]
	[MaxLength(2, ErrorMessage = "The state abbreviation must be exactly 2 characters long")]
	[MinLength(2, ErrorMessage = "The state abbreviation must be exactly 2 characters long")]
	public string Abbreviation { get; set; }
}