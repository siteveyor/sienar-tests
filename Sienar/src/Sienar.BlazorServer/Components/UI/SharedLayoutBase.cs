using Microsoft.AspNetCore.Components;

namespace Sienar.BlazorServer.Components.UI;

public class SharedLayoutBase : ComponentBase
{
	protected bool DrawerOpen { get; set; }

	protected void ToggleDrawer() => DrawerOpen = !DrawerOpen;
}