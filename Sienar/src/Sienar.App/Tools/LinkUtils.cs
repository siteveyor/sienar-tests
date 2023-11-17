using System.Text;
using Microsoft.AspNetCore.Http;

namespace Sienar.Tools;

public static class LinkUtils
{
	public static string GenerateTableFilterLink(
		string urlBase,
		string? searchTerm,
		string? sortName,
		bool? sortDescending,
		int? page,
		int? pageSize)
	{
		var newUrl = new StringBuilder($"{urlBase}?");
		if (!string.IsNullOrEmpty(searchTerm))
		{
			newUrl.Append($"searchTerm={searchTerm}&");
		}

		if (!string.IsNullOrEmpty(sortName))
		{
			newUrl.Append($"sortName={sortName}&");
		}

		if (sortDescending.HasValue)
		{
			newUrl.Append($"sortDescending={sortDescending.Value}&");
		}

		if (page.HasValue)
		{
			newUrl.Append($"page={page.Value}&");
		}

		if (pageSize.HasValue)
		{
			newUrl.Append($"pageSize={pageSize.Value}&");
		}

		// Remove trailing ampersand
		newUrl.Remove(newUrl.Length - 1, 1);

		return newUrl.ToString();
	}

	public static string GetRequestPath(this IHttpContextAccessor self)
	{
		return self.HttpContext!.Request.Path.ToString();
	}

	public static string? GetReturnUrl(this HttpRequest self)
	{
		return self.Query.ContainsKey("ReturnUrl")
			? self.Query["ReturnUrl"].ToString()
			: null;
	}
}