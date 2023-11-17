using Sienar.Infrastructure.Plugins;

namespace Sienar.Tools;

public static class RenderUtils
{
	public static string? Parse(CrossOriginMode? mode) => mode switch
	{
		CrossOriginMode.None => "none",
		CrossOriginMode.Anonymous => "anonymous",
		CrossOriginMode.UseCredentials => "use-credentials",
		_ => null
	};

	public static string? Parse(ReferrerPolicy? policy) => policy switch
	{
		ReferrerPolicy.NoReferrer => "no-referrer",
		ReferrerPolicy.NoReferrerWhenDowngrade => "no-referrer-when-downgrade",
		ReferrerPolicy.Origin => "origin",
		ReferrerPolicy.OriginWhenCrossOrigin => "origin-when-cross-origin",
		ReferrerPolicy.SameOrigin => "same-origin",
		ReferrerPolicy.StrictOrigin => "strict-origin",
		ReferrerPolicy.StrictOriginWhenCrossOrigin => "strict-origin-when-cross-origin",
		_ => null
	};

	public static string ParseIsJsModule(bool isModule) => isModule
		? "module"
		: "text/javascript";
}