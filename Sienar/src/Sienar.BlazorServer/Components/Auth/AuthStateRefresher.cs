using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sienar.Identity;

namespace Sienar.BlazorServer.Components.Auth;

public class AuthStateRefresher<TUser> : ComponentBase, IAsyncDisposable
{
	private const int Interval = 60 * 1000 * 10;
	private Timer? _timer;
	private bool _disposed;

	[Inject]
	private IBlazorServerSignInManager<TUser> SignInManager { get; set; } = default!;

	[Inject]
	private ILogger<AuthStateRefresher<TUser>> Logger { get; set; } = default!;

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			SetupRefreshTimer();
			await SignInManager.LoadUserLoginStatus();
		}
	}

	private void SetupRefreshTimer()
	{
		Logger.LogInformation("Updating refresh timer");
		_timer?.Dispose();
		_timer = new Timer(Interval);
		_timer.Elapsed += RefreshUserLogin;
		_timer.Enabled = true;
		_timer.Start();
	}

	private async void RefreshUserLogin(object? sender, EventArgs e)
	{
		Logger.LogInformation("Refreshing user login");
		await SignInManager.RefreshUserLoginStatus();
		SetupRefreshTimer();
	}

	/// <inheritdoc />
	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		await Dispose(true);
		GC.SuppressFinalize(this);
	}

	private async ValueTask Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}

		if (disposing)
		{
			if (_timer is not null)
			{
				_timer.Elapsed -= RefreshUserLogin;
				_timer.Dispose();
				await SignInManager.RefreshUserLoginStatus();
			}
		}

		_disposed = true;
	}
}