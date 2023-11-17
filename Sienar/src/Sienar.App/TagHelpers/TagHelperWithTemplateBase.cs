using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sienar.TagHelpers;

public abstract class TagHelperWithTemplateBase<TModel> : TagHelper
	where TModel : TemplateContentBase
{
	private readonly IHtmlHelper _htmlHelper;
	protected string TemplatePath { get; set; } = default!;

	[HtmlAttributeNotBound]
	[ViewContext]
	public ViewContext ViewContext { get; set; } = default!;

	protected TagHelperWithTemplateBase(IHtmlHelper htmlHelper)
	{
		_htmlHelper = htmlHelper;
	}

	public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
	{
		output.TagName = string.Empty;
		(_htmlHelper as IViewContextAware)!.Contextualize(ViewContext);

		var model = await GenerateModel(context);
		model.Content = await output.GetChildContentAsync();

		output.Content.SetHtmlContent(
			await _htmlHelper.PartialAsync(
				TemplatePath, 
				model));
	}

	protected abstract Task<TModel> GenerateModel(TagHelperContext context);
}