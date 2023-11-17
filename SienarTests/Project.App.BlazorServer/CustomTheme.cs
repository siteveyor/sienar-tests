using MudBlazor;

namespace Project.App.BlazorServer;

public class CustomTheme : MudTheme
{
	/// <inheritdoc />
	public CustomTheme()
	{
		Palette.AppbarBackground = Colors.Orange.Accent1;
	}
}