using System.Collections.Generic;

namespace Sienar.Infrastructure.Plugins;

public class StyleProvider : IStyleProvider
{
	protected readonly List<StyleResource> Resources = new();

	/// <inheritdoc />
	public IStyleProvider Enqueue(
		string url,
		CrossOriginMode? crossOrigin = null,
		ReferrerPolicy? referrerPolicy = null,
		string? integrity = null)
		=> Enqueue(
			new()
			{
				Url = url,
				Mode = crossOrigin,
				Referrer = referrerPolicy,
				Integrity = integrity
			});

	/// <inheritdoc />
	public IStyleProvider Enqueue(StyleResource resource)
	{
		Resources.Add(resource);
		return this;
	}

	/// <inheritdoc />
	public IEnumerable<StyleResource> GetStylesheets() => Resources;
}