using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sienar.Identity;
using Sienar.Infrastructure.Plugins;

namespace Sienar.Configuration;

public class SienarBuilder
{
	private readonly WebApplicationBuilder _builder;
	private readonly List<Action<WebApplication>> _pluginSetupAppFuncs = new();

	private SienarBuilder(WebApplicationBuilder builder)
	{
		_builder = builder;
	}

	public static SienarBuilder Create<TUser, TContext>(WebApplicationBuilder builder)
		where TUser : SienarUser
		where TContext : DbContext
	{
		var options = new SienarGenericTypeOptions
		{
			UserType = typeof(TUser),
			DbContextType = typeof(TContext)
		};

		builder.Services.AddSingleton(options);
		var sienarBuilder = new SienarBuilder(builder);

		return sienarBuilder;
	}

	public SienarBuilder AddPlugin<TPlugin>()
		where TPlugin : class, IPlugin
	{
		TPlugin.SetupDependencies(_builder);
		_builder.Services.AddScoped<IPlugin, TPlugin>();
		_pluginSetupAppFuncs.Add(TPlugin.SetupApp);
		return this;
	}

	public WebApplication Build()
	{
		var app = _builder.Build();

		foreach (var pluginSetupFunc in _pluginSetupAppFuncs)
		{
			pluginSetupFunc(app);
		}

		return app;
	}
}