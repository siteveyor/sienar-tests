using System.Collections.Generic;

namespace Sienar.Infrastructure;

public class NavLink
{
	/// <summary>
	/// The display text of the link
	/// </summary>
	public required string Text { get; set; }

	/// <summary>
	/// The URL the link points to
	/// </summary>
	public required string Url { get; set; }

	/// <summary>
	/// The icon to show along with the link, if any
	/// </summary>
	public string? Icon { get; set; }

	/// <summary>
	/// Whether the authorization requirements stored in <see cref="Roles"/> should be satisfied by all roles in the list being present, or only by a single role being present. Defaults to <c>true</c>, which requires all roles to be present.
	/// </summary>
	public bool AllRolesRequired { get; set; } = true;

	/// <summary>
	/// The role(s) required to see the link in the menu
	/// </summary>
	public IEnumerable<string>? Roles { get; set; }

	/// <summary>
	/// Child links to display in a submenu, if any
	/// </summary>
	public IEnumerable<NavLink>? Sublinks;
}