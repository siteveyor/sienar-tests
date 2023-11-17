using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sienar.Infrastructure.Plugins;

namespace Sienar.Middleware;

public class SienarPluginMiddleware
{
	private readonly RequestDelegate _next;

	public SienarPluginMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IEnumerable<IPlugin> plugins)
	{
		foreach (var plugin in plugins)
		{
			await plugin.Setup();
		}

		await _next(context);
	}
}