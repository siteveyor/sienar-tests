using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sienar.Constants;
using Sienar.Infrastructure;
using Sienar.Infrastructure.Plugins;
using Sienar.Middleware;

namespace Sienar.Configuration;

public class SienarCorePlugin : IPlugin
{
	private static readonly Version Version = Version.Parse("0.1.0");

	/// <inheritdoc />
	public PluginData PluginData { get; } = new()
	{
		Name = "Sienar Core",
		Version = Version,
		Author = "Christian LeVesque",
		AuthorUrl = "https://levesque.dev",
		Description = "Sienar Core provides all of the main services and configuration required to operate the Sienar CMS. Sienar cannot function without this plugin active.",
		Homepage = "https://sienar.siteveyor.com"
	};

	/// <inheritdoc />
	public static void SetupDependencies(WebApplicationBuilder builder)
	{
		var services = builder.Services;
		var config = builder.Configuration;
		var typeOptions = services.GetSienarGenerictypeOptions();

		services.AddScoped<IJsonSerializer, SienarJsonSerializer>();

		services
			.AddSienarUtilities()
			.AddSienarIdentity(typeOptions)
			.AddSienarMedia(typeOptions)
			.AddSienarStates(typeOptions)
			.ConfigureSienarOptions(config)
			.ConfigureSienarRazorPages()
			.ConfigureSienarAuth(typeOptions);
	}

	public static void SetupApp(WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Error");
		}

		app
			.UseStaticFiles()
			.UseRouting()
			.UseAuthentication()
			.UseAuthorization()
			.UseMiddleware<SienarSessionCookieMiddleware>()
			.UseMiddleware<SienarPluginMiddleware>();
		app.MapRazorPages();
		app.MapControllers();

		var styleProvider = app.Services.GetRequiredService<IStyleProvider>();
		EnqueueStyles(styleProvider);

		var scriptProvider = app.Services.GetRequiredService<IScriptProvider>();
		EnqueueScripts(scriptProvider);

		var moduleProvider = app.Services.GetRequiredService<IModuleImportMapProvider>();
		AddImportMaps(moduleProvider);

		var menuProvider = app.Services.GetRequiredService<IMenuProvider>();
		AddDashboardLinks(menuProvider);
	}

	private static void EnqueueStyles(IStyleProvider styleProvider)
	{
		styleProvider.Enqueue("https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100;0,300;0,400;0,500;0,700;0,900;1,100;1,300;1,400;1,500;1,700;1,900&display=swap");

		styleProvider.Enqueue(
			"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css",
			crossOrigin: CrossOriginMode.Anonymous,
			referrerPolicy: ReferrerPolicy.NoReferrer,
			integrity: "sha512-z3gLpd7yknf1YoNbCzqRKc4qyor8gaKU1qmn+CShxbuBusANI9QpRohGBreCFkKxLhei6S9CQXFEbbKuqLg0DA==");

		styleProvider.Enqueue(
			"https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css",
			crossOrigin: CrossOriginMode.Anonymous,
			referrerPolicy: ReferrerPolicy.NoReferrer,
			integrity: "sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN");

		styleProvider.Enqueue("/_content/Sienar.App/sienar.css");
	}

	private static void EnqueueScripts(IScriptProvider scriptProvider)
	{
		scriptProvider.Enqueue(
			$"/_content/Sienar.App/main.js?v={Version}",
			isModule: true);
	}

	private static void AddImportMaps(IModuleImportMapProvider moduleProvider)
	{
		moduleProvider
			.Add("bootstrap", "https://esm.sh/bootstrap@5.3.0")
			.Add("@sienar/razor-client", $"/_content/Sienar.App/sienar-razor-client.mjs?v={Version}");
	}

	private static void AddDashboardLinks(IMenuProvider menuProvider)
	{
		menuProvider.AddDashboardLink(
			SienarMenuNames.Dashboard,
			new () 
			{
				Text = "My account",
				Url = Urls.Dashboard.Account.EmailChange.EmailChangeIndex,
				Icon = "fa-solid fa-user",
				Roles = new [] { Roles.Admin },
				Sublinks = new NavLink[]
				{
					new ()
					{
						Text = "Change email",
						Url = Urls.Dashboard.Account.EmailChange.EmailChangeIndex
					},
					new ()
					{
						Text = "Change password",
						Url = Urls.Dashboard.Account.PasswordChange.PasswordChangeIndex
					},
					new ()
					{
						Text = "Personal data",
						Url = Urls.Dashboard.Account.PersonalData
					}
				}
			},
			10);

		menuProvider.AddDashboardLink(
			SienarMenuNames.Dashboard,
			new()
			{
				Text = "Plugins",
				Url = Urls.Dashboard.Plugins,
				Icon = "fa-solid fa-plug",
				Roles = new [] { Roles.Admin }
			},
			10);

		menuProvider.AddDashboardLink(
			SienarMenuNames.Dashboard,
			new ()
			{
				Text = "Users",
				Url = Urls.Dashboard.Users.UsersIndex,
				Icon = "fa-solid fa-users",
				Roles = new [] { Roles.Admin },
				Sublinks = new NavLink[]
				{
					new()
					{
						Text = "Users listing",
						Url = Urls.Dashboard.Users.UsersIndex
					},
					new ()
					{
						Text = "Add new user",
						Url = Urls.Dashboard.Users.Add
					}
				}
			},
			10);

		menuProvider.AddDashboardLink(
			SienarMenuNames.Dashboard,
			new ()
			{
				Text = "States",
				Url = Urls.Dashboard.States.StatesIndex,
				Icon = "fa-solid fa-building-columns",
				Roles = new [] { Roles.Admin },
				Sublinks = new NavLink[]
				{
					new()
					{
						Text = "States listing",
						Url = Urls.Dashboard.States.StatesIndex
					},
					new ()
					{
						Text = "Add new state",
						Url = Urls.Dashboard.States.Add
					}
				}
			},
			10);
	}
}