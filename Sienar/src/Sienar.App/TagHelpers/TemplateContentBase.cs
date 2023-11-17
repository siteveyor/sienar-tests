using Microsoft.AspNetCore.Html;

namespace Sienar.TagHelpers;

public abstract class TemplateContentBase
{
	public IHtmlContent? Content { get; set; }
}