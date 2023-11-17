using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Sienar.Infrastructure;

namespace Sienar.RazorComponents;

public class IndexTable<T> : ComponentBase
{
	[Parameter]
	public Filter Filter { get; set; } = default!;

	[Inject]
	private ICrudService<T> Service { get; set; } = default!;

	protected PagedDto<T> Items = new();

	/// <inheritdoc />
	protected override async Task OnInitializedAsync()
	{
		var result = await Service.Get(Filter);
		if (result.WasSuccessful)
		{
			Items = result.Result!;
		}
	}
}