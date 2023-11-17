using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor;
using Sienar.BlazorServer.Infrastructure;
using Sienar.Identity;

namespace Sienar.BlazorServer.Configuration;

public static class BlazorServerServiceCollectionExtensions
{
	public static IServiceCollection AddSienarBlazorServerAuth(
		this IServiceCollection self,
		Action<AuthorizationOptions>? authorizationOptions = null)
	{
		var typeOptions = self.GetSienarGenerictypeOptions();
		var signInManagerType = typeof(ISignInManager<>).MakeGenericType(typeOptions.UserType);

		self.TryAddScoped(
			signInManagerType,
			typeof(BlazorServerSignInManager<,>)
				.MakeGenericType(typeOptions.UserType, typeOptions.UserDtoType));

		self.TryAddScoped(
			typeof(IBlazorServerSignInManager<>)
				.MakeGenericType(typeOptions.UserType),
			sp => sp.GetRequiredService(signInManagerType));

		var defaultAuthStateProvider = self.FirstOrDefault(s => s.ImplementationType == typeof(AuthenticationStateProvider));
		if (defaultAuthStateProvider is not null)
		{
			self.Remove(defaultAuthStateProvider);
		}

		self.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
		self.AddScoped<AuthStateProvider>(
			sp => (AuthStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
		self.AddScoped<AccountStateProvider>();

		self.TryAddSingleton<MudTheme, SienarTheme>();

		return self.AddAuthorization(authorizationOptions ?? delegate {});
	}

	public static IServiceCollection AddBlazorServerOptions(
		this IServiceCollection self,
		IConfiguration config)
	{
		return self.Configure<SiteOptions>(config.GetSection("Sienar:Site"));
	}
}