using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Sienar.BlazorServer.Infrastructure;

namespace Sienar.BlazorServer.Components.Document;

public partial class DocTitle
{
	private string? _subtitle;

	[Inject]
	private IOptions<SiteOptions> Options { get; set; } = default!;

	[Parameter]
	public string? Title { get; set; }

	[Parameter]
#pragma warning disable BL0007
	public string Subtitle
	{
		get => _subtitle ?? Options.Value.Name;
		set => _subtitle = value;
	}
#pragma warning restore BL0007

	private string ComputedTitle =>
		string.IsNullOrEmpty(Title)
			? Subtitle
			: $"{Title} | {Subtitle}";
}