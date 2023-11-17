using System.Collections.Generic;

namespace Sienar.Infrastructure.Plugins;

public class ScriptProvider : IScriptProvider
{
	protected readonly List<ScriptResource> Resources = new();

	/// <inheritdoc />
	public IScriptProvider Enqueue(
		string url,
		bool isModule = false,
		bool isAsync = false,
		bool shouldDefer = true,
		CrossOriginMode? crossOrigin = null,
		ReferrerPolicy? referrerPolicy = null,
		string? integrity = null)
		=> Enqueue(
			new()
			{
				Url = url,
				IsModule = isModule,
				IsAsync = isAsync,
				ShouldDefer = shouldDefer,
				Mode = crossOrigin,
				Referrer = referrerPolicy,
				Integrity = integrity
			});

	/// <inheritdoc />
	public IScriptProvider Enqueue(ScriptResource resource)
	{
		Resources.Add(resource);
		return this;
	}

	/// <inheritdoc />
	public IEnumerable<ScriptResource> GetScripts() => Resources;
}