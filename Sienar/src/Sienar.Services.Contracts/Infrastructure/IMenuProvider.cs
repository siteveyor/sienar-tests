using System.Collections.Generic;

namespace Sienar.Infrastructure;

public interface IMenuProvider
{
	/// <summary>
	/// Gets the registered <see cref="NavLink"/> in the app, organized into named menus and grouped by priority
	/// </summary>
	Dictionary<string, Dictionary<int, List<NavLink>>> Menus { get; }

	/// <summary>
	/// Adds a navigation link to the dashboard menu at the given priority
	/// </summary>
	/// <param name="menuName">The name of the menu to add the nav link to</param>
	/// <param name="navLink">The nav link to add to the menu</param>
	/// <param name="priority">The priority at which to add the nav link to the menu</param>
	void AddDashboardLink(string menuName, NavLink navLink, int priority);
}