using System.Collections.Generic;
using System.Linq;

namespace Sienar.Infrastructure;

public class MenuGenerator : IMenuGenerator
{
	protected readonly IUserAccessor UserAccessor;
	protected readonly IMenuProvider MenuProvider;

	public MenuGenerator(IUserAccessor userAccessor, IMenuProvider menuProvider)
	{
		UserAccessor = userAccessor;
		MenuProvider = menuProvider;
	}

	/// <inheritdoc/>
	public IEnumerable<NavLink> CreateMenu(string menuName)
	{
		var orderedLinks = new List<NavLink>();
		foreach (var i in MenuProvider.Menus[menuName].Keys.Order())
		{
			orderedLinks.AddRange(MenuProvider.Menus[menuName][i]);
		}
		return ProcessNavLinks(orderedLinks);
	}

	protected IEnumerable<NavLink> ProcessNavLinks(IEnumerable<NavLink> navLinks)
	{
		var includedLinks = new List<NavLink>();

		foreach (var link in navLinks)
		{
			if (!UserIsAuthorized(link))
			{
				continue;
			}

			if (link.Sublinks is not null)
			{
				link.Sublinks = ProcessNavLinks(link.Sublinks);
			}

			includedLinks.Add(link);
		}

		return includedLinks;
	}

	protected bool UserIsAuthorized(NavLink navLink)
	{
		if (navLink.Roles is null)
		{
			return true;
		}

		foreach (var role in navLink.Roles)
		{
			if (UserAccessor.UserInRole(role))
			{
				if (navLink.AllRolesRequired)
				{
					continue;
				}

				return true;
			}

			if (navLink.AllRolesRequired)
			{
				return false;
			}
		}

		return navLink.AllRolesRequired;
	}
}