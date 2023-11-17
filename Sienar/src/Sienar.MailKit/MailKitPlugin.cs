using System;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sienar.Configuration;
using Sienar.Infrastructure.Plugins;

namespace Sienar.Email;

public class MailKitPlugin : IPlugin
{
	/// <inheritdoc />
	public static void SetupDependencies(WebApplicationBuilder builder)
	{
		var services = builder.Services;

		services.AddScoped<ISmtpClient>(delegate { return new SmtpClient(); });

		services.RemoveService<IEmailSender>();
		services.AddScoped<IEmailSender, MailKitSender>();

		var smtpConfigurer = services.GetAndRemoveService<Action<SmtpOptions>>();
		if (smtpConfigurer is null)
		{
			services.Configure<SmtpOptions>(
				builder.Configuration.GetSection("Sienar:Smtp"));
		}
		else
		{
			services.Configure(smtpConfigurer);
		}
	}

	/// <inheritdoc />
	public PluginData PluginData { get; } = new()
	{
		Name = "Sienar MailKit",
		Version = Version.Parse("0.1.0"),
		Author = "Christian LeVesque",
		AuthorUrl = "https://levesque.dev",
		Description = "Sienar MailKit provides access to mail delivery services over SMTP using the MailKit library for .NET.",
		Homepage = "https://sienar.siteveyor.com/plugins/mailkit"
	};
}