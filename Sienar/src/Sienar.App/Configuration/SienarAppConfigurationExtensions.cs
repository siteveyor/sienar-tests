using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RazorForms.Options;
using Sienar.Authorization;
using Sienar.Email;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Infrastructure.Plugins;
using Sienar.Infrastructure.States;

namespace Sienar.Configuration;

public static class SienarAppConfigurationExtensions
{
	public static IServiceCollection AddSienarUtilities(this IServiceCollection self)
	{
		self.AddHttpContextAccessor();

		self.TryAddScoped<IBotDetector, BotDetector>();
		self.TryAddScoped<IEmailSender, DefaultEmailSender>();
		self.TryAddSingleton<IToastService, ToastService>();
		self.TryAddSingleton<IScriptProvider, ScriptProvider>();
		self.TryAddSingleton<IStyleProvider, StyleProvider>();
		self.TryAddSingleton<IModuleImportMapProvider, ModuleImportMapProvider>();
		self.TryAddScoped<IMenuGenerator, MenuGenerator>();
		self.TryAddSingleton<IMenuProvider, MenuProvider>();

		return self;
	}

	public static IServiceCollection AddSienarIdentity(
		this IServiceCollection self,
		SienarGenericTypeOptions typeOptions)
	{
		self.AddSienarCrudService(
			typeOptions.UserType,
			typeof(SienarUserDto),
			typeof(UserDtoAdapter<,>)
				.MakeGenericType(
					typeOptions.UserType,
					typeof(SienarUserDto)),
			typeof(IAdminAccountService),
			typeof(AdminAccountService<,>)
				.MakeGenericType(
					typeOptions.UserType,
					typeOptions.DbContextType));

		self.AddSienarCrudService(
			typeof(SienarRole),
			typeof(SienarRoleDto),
			typeof(RoleDtoAdapter),
			typeof(IRoleService),
			typeof(RoleService<>)
				.MakeGenericType(typeOptions.DbContextType));

		self.TryAddScoped(
			typeof(IAccountService),
			typeof(AccountService<,,>)
				.MakeGenericType(
					typeOptions.UserType,
					typeof(SienarUserDto),
					typeOptions.DbContextType));

		self.TryAddScoped(
			typeof(IFilterProcessor<>)
				.MakeGenericType(typeOptions.UserType),
			typeof(SienarUserFilterProcessor<>)
				.MakeGenericType(typeOptions.UserType));
		self.TryAddScoped(
			typeof(IPasswordHasher<>)
				.MakeGenericType(typeOptions.UserType),
			typeof(PasswordHasher<>)
				.MakeGenericType(typeOptions.UserType));
		self.TryAddScoped(
			typeof(IUserClaimsFactory<>)
				.MakeGenericType(typeOptions.UserType),
			typeof(UserClaimsFactory<>)
				.MakeGenericType(typeOptions.UserType));
		self.TryAddScoped(
			typeof(IUserClaimsPrincipalFactory<>)
				.MakeGenericType(typeOptions.UserType),
			typeof(Identity.UserClaimsPrincipalFactory<>)
				.MakeGenericType(typeOptions.UserType));
		self.TryAddScoped(
			typeof(IVerificationCodeManager<>)
				.MakeGenericType(typeOptions.UserType),
			typeof(VerificationCodeManager<,>)
				.MakeGenericType(
					typeOptions.UserType,
					typeOptions.DbContextType));
		self.TryAddScoped(
			typeof(IUserManager<>)
				.MakeGenericType(typeOptions.UserType),
			typeof(UserManager<,>)
				.MakeGenericType(
					typeOptions.UserType,
					typeOptions.DbContextType));

		self.TryAddScoped<IUserAccessor, UserAccessor>();
		self.TryAddScoped<IAccountEmailMessageFactory, AccountEmailMessageFactory>();
		self.TryAddScoped<IAccountEmailManager, AccountEmailManager>();
		self.TryAddScoped<IAccountUrlProvider, AccountUrlProvider>();

		return self;
	}

	public static IServiceCollection AddSienarMedia(
		this IServiceCollection self,
		SienarGenericTypeOptions typeOptions)
	{
		self.AddSienarCrudService(
			typeof(Medium),
			typeof(MediumDto),
			typeof(MediaDtoAdapter),
			typeof(IMediaService),
			typeof(MediaService<>)
				.MakeGenericType(typeOptions.DbContextType));

		self.TryAddScoped<IFilterProcessor<Medium>, MediumFilterProcessor>();
		self.TryAddScoped<IMediaDirectoryMapper, MediaDirectoryMapper>();
		self.TryAddScoped<IMediaManager, MediaManager>();

		return self;
	}

	public static IServiceCollection AddSienarStates(
		this IServiceCollection self,
		SienarGenericTypeOptions typeOptions)
	{
		self.AddSienarCrudService(
			typeof(State),
			typeof(StateDto),
			typeof(StateDtoAdapter),
			typeof(IStateService),
			typeof(StateService<>)
				.MakeGenericType(typeOptions.DbContextType));
		self.TryAddScoped<IFilterProcessor<State>, StateFilterProcessor>();

		return self;
	}

	public static IServiceCollection ConfigureSienarOptions(
		this IServiceCollection self,
		IConfiguration config)
	{
		self
			.Configure<SienarOptions>(config.GetSection("Sienar:Core"))
			.Configure<EmailOptions>(config.GetSection("Sienar:Email:Core"))
			.Configure<IdentityEmailOptions>(config.GetSection("Sienar:Email:IdentityEmailSubjects"))
			.Configure<LoginOptions>(config.GetSection("Sienar:Login"));

		return self;
	}

	public static IServiceCollection ConfigureSienarRazorPages(this IServiceCollection self)
	{
		var controllersConfigurer = self.GetAndRemoveService<Action<MvcOptions>>();
		var razorPagesConfigurer = self.GetAndRemoveService<Action<RazorPagesOptions>>();
		var apiBehaviorConfigurer = self.GetAndRemoveService<Action<ApiBehaviorOptions>>();

		self.AddControllers(controllersConfigurer);
		self.AddRazorPages(
			o =>
			{
				o.Conventions
					.AuthorizeFolder("/Dashboard")
					.AuthorizeFolder("/Dashboard/Users", Policies.IsAdmin);
				razorPagesConfigurer?.Invoke(o);
			})
			.ConfigureApiBehaviorOptions(
				o =>
				{
					o.InvalidModelStateResponseFactory = ctx =>
					{
						var problemDetails = new ValidationProblemDetails(ctx.ModelState)
						{
							Type = "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2",
							Title = "One or more model validation errors occurred.",
							Status = StatusCodes.Status422UnprocessableEntity,
							Detail = "See the errors property for details",
							Instance = ctx.HttpContext.Request.Path
						};

						problemDetails.Extensions.Add(
							"traceId",
							ctx.HttpContext.TraceIdentifier);

						return new UnprocessableEntityObjectResult(problemDetails) { ContentTypes = { "application/json" } };
					};

					apiBehaviorConfigurer?.Invoke(o);
				});

		var razorFormsConfiguer = self.GetAndRemoveService<Action<RazorFormsOptions>>();

		self.UseRazorFormsWithBootstrap5FloatingLabels(
			o =>
			{
				// Text
				o.TextInputOptions.ComponentWrapperClasses = "mb-3";
				o.TextInputOptions.ErrorWrapperClasses = "mt-1";
				o.TextInputOptions.ErrorClasses = "small";
				o.TextInputOptions.AlwaysRenderErrorContainer = true;

				// TextArea
				o.TextAreaInputOptions.ComponentWrapperClasses = "mb-3";
				o.TextAreaInputOptions.ErrorWrapperClasses = "mt-1";
				o.TextAreaInputOptions.ErrorClasses = "small";
				o.TextAreaInputOptions.AlwaysRenderErrorContainer = true;

				// Select
				o.SelectInputOptions.ComponentWrapperClasses = "mb-3";
				o.SelectInputOptions.ErrorWrapperClasses = "mt-1";
				o.SelectInputOptions.ErrorClasses = "small";
				o.SelectInputOptions.AlwaysRenderErrorContainer = true;

				// Radio/checkbox
				o.CheckInputGroupOptions.ComponentWrapperClasses = "mb-3";
				o.CheckInputGroupOptions.AlwaysRenderErrorContainer = true;
				o.RadioInputGroupOptions.ComponentWrapperClasses = "mb-3";
				o.RadioInputGroupOptions.AlwaysRenderErrorContainer = true;

				// Dev-supplied configuration
				razorFormsConfiguer?.Invoke(o);
			});

		return self;
	}

	public static IServiceCollection ConfigureSienarAuth(
		this IServiceCollection self,
		SienarGenericTypeOptions typeOptions)
	{
		self.TryAddScoped(
			typeof(ISignInManager<>)
				.MakeGenericType(typeOptions.UserType),
			typeof(CookieSignInManager<>)
				.MakeGenericType(typeOptions.UserType));
		self.AddScoped<IAuthorizationHandler, InRoleHandler>();

		var authorizationConfigurer = self.GetAndRemoveService<Action<AuthorizationOptions>>();
		var authenticationConfigurer = self.GetAndRemoveService<Action<AuthenticationOptions>>();
		var cookieAuthenticationConfigurer = self.GetAndRemoveService<Action<CookieAuthenticationOptions>>();

		self
			.AddAuthorization(
				o =>
				{
					authorizationConfigurer?.Invoke(o);
					o.AddPolicy(
						Policies.IsAdmin,
						p => p.Requirements.Add(new InRoleRequirement(Roles.Admin)));
				})
			.AddAuthentication(
				o =>
				{
					authenticationConfigurer?.Invoke(o);
				})
			.AddCookie(
				CookieAuthenticationDefaults.AuthenticationScheme,
				o =>
				{
					cookieAuthenticationConfigurer?.Invoke(o);
				});

		return self;
	}
}