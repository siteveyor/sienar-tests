namespace Sienar.TagHelpers.Forms;

public class FormCardModel : TemplateContentBase
{
	private string? _submitText;
	private string? _resetText;
	private string? _formStyle;
	private string? _submitButtonStyle;
	private string? _resetButtonStyle;
	private string? _linkTextStyle;

	public string Title { get; set; } = default!;
	public string? Subtitle { get; set; }
	public string? LinkPage { get; set; }
	public string? LinkText { get; set; }
	public string? ErrorMessage { get; set; }
	public bool HideResetButton { get; set; }

	public string SubmitText
	{
		get => _submitText ?? "Submit";
		set => _submitText = value;
	}

	public string ResetText
	{
		get => _resetText ?? "Reset";
		set => _resetText = value;
	}

	public string FormColor
	{
		get => _formStyle ?? "primary";
		set => _formStyle = value;
	}

	public string SubmitButtonStyle
	{
		get => _submitButtonStyle ?? "primary";
		set => _submitButtonStyle = value;
	}

	public string ResetButtonStyle
	{
		get => _resetButtonStyle ?? "outline-secondary";
		set => _resetButtonStyle = value;
	}

	public string LinkTextColor
	{
		get => _linkTextStyle ?? string.Empty;
		set => _linkTextStyle = value;
	}
}