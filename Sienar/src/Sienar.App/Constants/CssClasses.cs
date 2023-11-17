namespace Sienar.Constants;

public static class CssClasses
{
	public const string DashboardDarkButton = $"btn btn-dark {OffWhiteDarkBgText}";
	public const string OffWhiteDarkBgText = "text-white text-opacity-75";

	public class Layouts
	{
		public const string Body = "d-flex flex-column min-vh-100";
		public const string Main = "flex-grow-1 position-relative";
		public const string DashboardMain = $"{Main} d-flex";
		public const string BroadContainer = $"py-5 mx-auto w-100";
		public const string NarrowContainer = "py-5 mx-auto narrow-layout";
		public const string DashboardContainer = "flex-grow-1 d-flex flex-column";
		public const string DashboardBroadContainer = $"{BroadContainer} flex-grow-1 px-5";
		public const string DashboardNarrowContainer = $"{NarrowContainer} flex-grow-1 px-5";
	}
}