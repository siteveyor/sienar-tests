using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sienar.TagHelpers.Forms;

public class FormCardTagHelper : TagHelperWithTemplateBase<FormCardModel>
{
	[HtmlAttributeName("form-title")]
	public string Title { get; set; } = default!;

	[HtmlAttributeName("form-subtitle")]
	public string? Subtitle { get; set; }

	[HtmlAttributeName("form-color")]
	public string? FormColor { get; set; }

	[HtmlAttributeName("link-page")]
	public string? LinkPage { get; set; }

	[HtmlAttributeName("link-text")]
	public string? LinkText { get; set; }

	[HtmlAttributeName("link-text-color")]
	public string? LinkTextColor { get; set; }

	[HtmlAttributeName("form-error")]
	public string? ErrorMessage { get; set; }

	[HtmlAttributeName("submit-text")]
	public string? SubmitText { get; set; }

	[HtmlAttributeName("submit-button-style")]
	public string? SubmitButtonStyle { get; set; }

	[HtmlAttributeName("reset-text")]
	public string? ResetText { get; set; }

	[HtmlAttributeName("reset-button-style")]
	public string? ResetButtonStyle { get; set; }

	[HtmlAttributeName("hide-reset-button")]
	public bool HideResetButton { get; set; }

	public FormCardTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper)
	{
		TemplatePath = "~/TagHelpers/Forms/FormCardTemplate.cshtml";
	}

	protected override Task<FormCardModel> GenerateModel(TagHelperContext context)
	{
		var model = new FormCardModel
		{
			Title = Title,
			Subtitle = Subtitle,
			LinkPage = LinkPage,
			LinkText = LinkText,
			ErrorMessage = ErrorMessage,
			HideResetButton = HideResetButton,
			// Model guarantees default values internally
			SubmitText = SubmitText!,
			ResetText = ResetText!,
			FormColor = FormColor!,
			SubmitButtonStyle = SubmitButtonStyle!,
			ResetButtonStyle = ResetButtonStyle!,
			LinkTextColor = LinkTextColor!
		};

		return Task.FromResult(model);
	}
}