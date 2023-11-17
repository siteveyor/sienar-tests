#nullable disable

using System.Collections.Generic;

namespace Sienar.Identity;

public class SienarRole : EntityBase
{
	/// <summary>
	/// Represents the name of the role
	/// </summary>
	public string Name { get; set; }

	/// <inheritdoc/>
	public override string ToString() => Name;
}