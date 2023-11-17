using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Sienar.Tools;

namespace Sienar.Infrastructure;

public class ToastService : IToastService
{
	protected readonly Dictionary<Guid, List<ToastDto>> Toasts = new();
	protected readonly IHttpContextAccessor ContextAccessor;

	public ToastService(IHttpContextAccessor accessor)
	{
		ContextAccessor = accessor;
	}

	/// <inheritdoc />
	public void EnqueueToast(ToastDto toast)
	{
		var id = GetSessionId();
		if (!Toasts.ContainsKey(id))
		{
			Toasts[id] = new();
		}

		Toasts[id].Add(toast);
	}

	/// <inheritdoc />
	public void EnqueueToast(
		MessageType type,
		string bodyText,
		string? titleText = null,
		int? delay = null,
		bool isBackgroundTheme = true)
	{
		// Warnings and errors should require manual dismissal
		delay ??= type is MessageType.Warning or MessageType.Error
			? 0
			: 5000;

		EnqueueToast(
			new()
			{
				Type = type,
				BodyText = bodyText,
				TitleText = titleText,
				Delay = delay.Value,
				IsBackgroundTheme = isBackgroundTheme
			});
	}

	/// <inheritdoc />
	public IEnumerable<ToastDto> GetToasts()
	{
		var id = GetSessionId();
		if (!Toasts.ContainsKey(id))
		{
			return Array.Empty<ToastDto>();
		}

		var toasts = Toasts[id];
		Toasts.Remove(id);
		return toasts;
	}

	/// <inheritdoc />
	public void Default(string bodyText, string? titleText = null)
		=> EnqueueToast(MessageType.Default, bodyText, titleText);

	/// <inheritdoc />
	public void Success(string bodyText, string? titleText = null)
		=> EnqueueToast(MessageType.Success, bodyText, titleText);

	/// <inheritdoc />
	public void Error(string bodyText, string? titleText = null)
		=> EnqueueToast(MessageType.Error, bodyText, titleText);

	/// <inheritdoc />
	public void Warning(string bodyText, string? titleText = null)
		=> EnqueueToast(MessageType.Warning, bodyText, titleText);

	/// <inheritdoc />
	public void Info(string bodyText, string? titleText = null)
		=> EnqueueToast(MessageType.Info, bodyText, titleText);

	/// <inheritdoc />
	public void Blank(string bodyText, string? titleText = null)
		=> EnqueueToast(MessageType.None, bodyText, titleText);

	protected Guid GetSessionId() => ContextAccessor.HttpContext!.GetSienarSessionId()!.Value;
}