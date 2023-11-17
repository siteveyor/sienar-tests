using System.Collections.Generic;

namespace Sienar.Infrastructure;

public class MenuProvider : IMenuProvider
{
	/// <inheritdoc />
	public Dictionary<string,Dictionary<int, List<NavLink>>> Menus { get; } = new();

	/// <inheritdoc />
	public void AddDashboardLink(string menuName, NavLink navLink, int priority)
	{
		if (!Menus.ContainsKey(menuName))
		{
			Menus[menuName] = new();
		}

		if (!Menus[menuName].ContainsKey(priority))
		{
			Menus[menuName][priority] = new ();
		}

		Menus[menuName][priority].Add(navLink);
	}
}