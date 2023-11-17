using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sienar.Tools;

namespace Sienar.Middleware;

public class SienarSessionCookieMiddleware
{
	private readonly RequestDelegate _next;

	public SienarSessionCookieMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var sessionId = context.Request.GetSienarSessionId();
		if (!sessionId.HasValue)
		{
			context.Response.Cookies.Append(
				CookieUtils.SienarSessionIdCookieName,
				Guid.NewGuid().ToString(),
				new ()
				{
					HttpOnly = true,
					IsEssential = true,
					SameSite = SameSiteMode.Strict 
				});
		}

		await _next(context);
	}
}