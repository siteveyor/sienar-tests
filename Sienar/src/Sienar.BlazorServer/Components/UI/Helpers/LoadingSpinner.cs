using MudBlazor;

namespace Sienar.BlazorServer.Components.UI.Helpers;

public class LoadingSpinner : MudProgressCircular
{
	public LoadingSpinner()
	{
		Color = Color.Primary;
		Indeterminate = true;
	}
}