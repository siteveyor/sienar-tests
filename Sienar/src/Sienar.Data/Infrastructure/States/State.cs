#nullable disable

using System.ComponentModel.DataAnnotations;
using Sienar;
using Microsoft.EntityFrameworkCore;

namespace Sienar.Infrastructure.States;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Abbreviation), IsUnique = true)]
public class State : EntityBase
{
	[MaxLength(20)]
	[Required]
	public string Name { get; set; }

	[MaxLength(2)]
	[Required]
	public string Abbreviation { get; set; }
}