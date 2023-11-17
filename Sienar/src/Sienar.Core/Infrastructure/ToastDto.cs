namespace Sienar.Infrastructure;

public class ToastDto
{
	/// <summary>
	/// The theme to use for the toast
	/// </summary>
	public MessageType Type { get; set; }

	/// <summary>
	/// Whether the theme should target the foreground or the background
	/// </summary>
	public bool IsBackgroundTheme { get; set; }

	/// <summary>
	/// The text to display in the toast header
	/// </summary>
	public string? TitleText { get; set; }

	/// <summary>
	/// The text to display in the toast body
	/// </summary>
	public required string BodyText { get; set; }

	/// <summary>
	/// The duration (in ms) for which the toast should be shown
	/// </summary>
	/// <remarks>
	/// Set to 0 to disable auto-hide functionality.
	/// </remarks>
	public int Delay { get; set; } = 5000;
}