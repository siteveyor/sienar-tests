using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Data;
using Sienar.Configuration;
using Sienar.Email;

var builder = WebApplication.CreateBuilder(args);

// Add Sienar and customized Identity
builder.Services
	.AddDbContext<AppDbContext>(o =>
	{
		var cnx = builder.Configuration.GetConnectionString("Default");
		o.UseMySql(cnx, ServerVersion.AutoDetect(cnx));
	});

SienarBuilder
	.Create<AppUser, AppDbContext>(builder)
	.AddPlugin<SienarCorePlugin>()
	.AddPlugin<MailKitPlugin>()
	.Build()
	.Run();