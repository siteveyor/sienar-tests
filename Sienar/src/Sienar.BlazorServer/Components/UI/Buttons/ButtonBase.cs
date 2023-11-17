using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Sienar.BlazorServer.Components.UI.Buttons;

public class ButtonBase : MudButton
{
	public ButtonBase()
	{
		ButtonType = ButtonType.Button;
		Variant = Variant.Text;
	}
}