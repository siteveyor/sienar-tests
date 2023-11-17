using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sienar.TagHelpers.Cards;

public class CardTagHelper : TagHelperWithTemplateBase<CardModel>
{
	[HtmlAttributeName("card-title")]
	public string Title { get; set; } = default!;

	[HtmlAttributeName("card-subtitle")]
	public string? Subtitle { get; set; }

	/// <inheritdoc />
	public CardTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper)
	{
		TemplatePath = "~/TagHelpers/Cards/Template.cshtml";
	}

	/// <inheritdoc />
	protected override Task<CardModel> GenerateModel(TagHelperContext context)
	{
		var model = new CardModel
		{
			Title = Title,
			Subtitle = Subtitle
		};

		return Task.FromResult(model);
	}
}