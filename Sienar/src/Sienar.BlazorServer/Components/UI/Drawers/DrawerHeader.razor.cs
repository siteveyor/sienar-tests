using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Sienar.BlazorServer.Infrastructure;
using Sienar.Identity;

namespace Sienar.BlazorServer.Components.UI.Drawers;

public partial class DrawerHeader : IDisposable
{
	[Inject]
	private AccountStateProvider Account { get; set; } = default!;

	[Inject]
	private IOptions<SiteOptions> Options { get; set; } = default!;

	private SienarUserDto? User => Account.User;

	protected override void OnInitialized()
	{
		Account.OnChange += StateHasChanged;
	}

	public void Dispose()
	{
		Account.OnChange -= StateHasChanged;
	}
}