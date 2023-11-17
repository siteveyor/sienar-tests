using System.Collections.Generic;

namespace Sienar.Infrastructure;

public interface IMenuGenerator
{
	/// <summary>
	/// Creates a list of <see cref="NavLink"/> to be rendered in the dashboard menu
	/// </summary>
	/// <param name="menuName">The name of the menu to create</param>
	/// <returns>the list of <see cref="NavLink"/> to render</returns>
	IEnumerable<NavLink> CreateMenu(string menuName);
}