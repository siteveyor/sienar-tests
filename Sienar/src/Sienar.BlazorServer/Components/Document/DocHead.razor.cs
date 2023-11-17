using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Sienar.BlazorServer.Infrastructure;

namespace Sienar.BlazorServer.Components.Document;

public partial class DocHead
{
	[Parameter]
	public string? Description { get; set; }

	[Parameter]
	public string? Title { get; set; }

	[Parameter]
	public string? Subtitle { get; set; }

	[Parameter]
	public string? HeaderImage { get; set; }

	[Parameter]
	public string? ImageAlt { get; set; }

	[Inject]
	private NavigationManager NavManager { get; set; } = default!;

	[Inject]
	private IOptions<SiteOptions> SiteOptions { get; set; } = default!;

	// TODO: Add support for multiple types from OpenGraph. These should be determined automatically based on data passed into <DocHead>
	private string GenerateOgType() => "website";

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (string.IsNullOrEmpty(ImageAlt))
		{
			ImageAlt = $"{SiteOptions.Value.Name} header image";
		}
	}
}