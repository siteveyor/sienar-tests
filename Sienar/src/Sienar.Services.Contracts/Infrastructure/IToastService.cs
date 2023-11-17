using System.Collections.Generic;

namespace Sienar.Infrastructure;

public interface IToastService
{
	/// <summary>
	/// Registers a Toast to be displayed to the user
	/// </summary>
	/// <param name="toast">the <see cref="ToastDto"/> to enqueue</param>
	void EnqueueToast(ToastDto toast);

	/// <summary>
	/// Creates and registers a Toast to be displayed to the user
	/// </summary>
	/// <param name="type">The <see cref="MessageType"/></param>
	/// <param name="bodyText">The main message to display</param>
	/// <param name="titleText">The title text to display</param>
	/// <param name="delay">The duration the toast should be shown, in ms</param>
	/// <param name="isBackgroundtheme">Whether the color describes the foreground theme color or background theme color</param>
	void EnqueueToast(
		MessageType type,
		string bodyText,
		string? titleText = null,
		int? delay = null,
		bool isBackgroundtheme = true);

	/// <summary>
	/// Gets the list of all registered toasts
	/// </summary>
	/// <remarks>
	/// Calling this method will also clear all enqueued toasts for the current user.
	/// </remarks>
	/// <returns>the toasts</returns>
	IEnumerable<ToastDto> GetToasts();

	/// <summary>
	/// Registers a Toast with a <see cref="MessageType"/> of <c>Default</c>
	/// </summary>
	/// <param name="bodyText">The main message to display</param>
	/// <param name="titleText">The title text to display</param>
	void Default(string bodyText, string? titleText = null);

	/// <summary>
	/// Registers a Toast with a <see cref="MessageType"/> of <c>Success</c>
	/// </summary>
	/// <param name="bodyText">The main message to display</param>
	/// <param name="titleText">The title text to display</param>
	void Success(string bodyText, string? titleText = null);

	/// <summary>
	/// Registers a Toast with a <see cref="MessageType"/> of <c>Error</c>
	/// </summary>
	/// <param name="bodyText">The main message to display</param>
	/// <param name="titleText">The title text to display</param>
	void Error(string bodyText, string? titleText = null);

	/// <summary>
	/// Registers a Toast with a <see cref="MessageType"/> of <c>Warning</c>
	/// </summary>
	/// <param name="bodyText">The main message to display</param>
	/// <param name="titleText">The title text to display</param>
	void Warning(string bodyText, string? titleText = null);

	/// <summary>
	/// Registers a Toast with a <see cref="MessageType"/> of <c>Info</c>
	/// </summary>
	/// <param name="bodyText">The main message to display</param>
	/// <param name="titleText">The title text to display</param>
	void Info(string bodyText, string? titleText = null);

	/// <summary>
	/// Registers a Toast with a <see cref="MessageType"/> of <c>None</c>
	/// </summary>
	/// <param name="bodyText">The main message to display</param>
	/// <param name="titleText">The title text to display</param>
	void Blank(string bodyText, string? titleText = null);
}