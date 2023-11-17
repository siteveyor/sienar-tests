using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Project.App.BlazorServer;
using Project.Data;
using Sienar.BlazorServer.Configuration;
using Sienar.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<MudTheme, CustomTheme>();

builder.Services.AddDbContext<AppDbContext>(
	o =>
	{
		var cnx = builder.Configuration.GetConnectionString("Default");
		o.UseMySql(cnx, ServerVersion.AutoDetect(cnx));
	},
	ServiceLifetime.Transient);

builder.Services
	.AddMudServices()
	.AddAuthorization()
	.AddSienar<AppUser, AppDbContext>()
	.AddDefaultOptions(builder.Configuration)
	.AddMailKit(builder.Configuration);

builder.Services.AddSienarBlazorServerAuth();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
