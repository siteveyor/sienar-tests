namespace Sienar.TagHelpers.Cards;

public class CardModel : TemplateContentBase
{
	public string Title { get; set; } = default!;
	public string? Subtitle { get; set; }
}