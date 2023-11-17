using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sienar.Configuration;
using Sienar.Errors;
using Sienar.Infrastructure;

namespace Sienar.Identity;

public class AccountService<TUser, TUserDto, TContext>
	: IAccountService
	where TUser : SienarUser, new()
	where TUserDto : SienarUserDto
	where TContext : DbContext
{
	protected readonly TContext Context;
	protected readonly IDtoAdapter<TUser, TUserDto> UserDtoAdapter;
	protected readonly IVerificationCodeManager<TUser> VerificationCodeManager;
	protected readonly IUserManager<TUser> UserManager;
	protected readonly IPasswordHasher<TUser> PasswordHasher;
	protected readonly IAccountEmailManager EmailManager;
	protected readonly ISignInManager<TUser> SignInManager;
	protected readonly LoginOptions LoginOptions;
	protected readonly SienarOptions AppOptions;
	protected readonly IBotDetector BotDetector;
	protected readonly IUserAccessor UserAccessor;
	protected readonly IEnumerable<IUserPersonalDataRetriever<TUser>> PersonalDataRetrievers;

	protected DbSet<TUser> UserSet => Context.Set<TUser>();

	public AccountService(
		TContext context,
		IDtoAdapter<TUser, TUserDto> userDtoAdapter,
		IVerificationCodeManager<TUser> verificationCodeManager,
		IUserManager<TUser> userManager,
		IPasswordHasher<TUser> passwordHasher,
		IAccountEmailManager emailManager,
		ISignInManager<TUser> signInManager,
		IOptions<LoginOptions> loginOptions,
		IOptions<SienarOptions> appOptions,
		IBotDetector botDetector,
		IUserAccessor userAccessor,
		IEnumerable<IUserPersonalDataRetriever<TUser>> personalDataRetrievers)
	{
		Context = context;
		UserDtoAdapter = userDtoAdapter;
		VerificationCodeManager = verificationCodeManager;
		UserManager = userManager;
		PasswordHasher = passwordHasher;
		EmailManager = emailManager;
		SignInManager = signInManager;
		LoginOptions = loginOptions.Value;
		AppOptions = appOptions.Value;
		BotDetector = botDetector;
		UserAccessor = userAccessor;
		PersonalDataRetrievers = personalDataRetrievers;
	}

	/// <inheritdoc/>
	public virtual async Task<ServiceResult<bool>> Register(
		RegisterDto userData)
	{
		if (!AppOptions.RegistrationOpen)
		{
			return ServiceResult<bool>.Unprocessable(ErrorMessages.Account.RegistrationDisabled);
		}

		BotDetector.DetectSpambot(userData);

		// Verify they accepted the terms and conditions
		if (!userData.AcceptTos)
		{
			return ServiceResult<bool>.Unprocessable(ErrorMessages.Account.MustAcceptTos);
		}

		// Check for duplicates
		var existingUser = await UserSet.FirstOrDefaultAsync(u => u.Username == userData.Username);
		if (existingUser != null)
		{
			return ServiceResult<bool>.Unprocessable(ErrorMessages.Account.UsernameTaken);
		}

		existingUser = await UserSet.FirstOrDefaultAsync(
				u => u.Email == userData.Email
				|| u.PendingEmail == userData.Email);
		if (existingUser != null)
		{
			return ServiceResult<bool>.Unprocessable(ErrorMessages.Account.EmailTaken);
		}

		// Checks passed. Make a new user
		var user = new TUser
		{
			Username = userData.Username,
			Email = userData.Email,
			ConcurrencyStamp = Guid.NewGuid()
		};

		user.PasswordHash = PasswordHasher.HashPassword(user, userData.Password);

		var shouldSendRegistrationEmail = ShouldSendEmailConfirmationEmail();
		if (!shouldSendRegistrationEmail)
		{
			user.EmailConfirmed = true;
		}

		// Try to create that user with the given password
		await UserSet.AddAsync(user);
		await Context.SaveChangesAsync();

		if (shouldSendRegistrationEmail)
		{
			await SendWelcomeEmail(user);
		}

		return ServiceResult<bool>.Ok("Registered successfully");
	}

	/// <inheritdoc/>
	public virtual async Task<ServiceResult<bool>> Login(LoginDto login)
	{
		BotDetector.DetectSpambot(login);

		var user = await UserManager.GetUser(
			login.AccountName,
			u => u.Roles);
		if (user == null)
		{
			return ServiceResult<bool>.NotFound(
				ErrorMessages.Account.LoginFailedNotFound);
		}

		// If user is locked out, tell them when the lockout date ends
		if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now)
		{
			return ServiceResult<bool>.Unauthorized(ErrorMessages.Account.GetLockoutMessage(user.LockoutEnd));
		}

		if (!await UserManager.VerifyPassword(user, login.Password))
		{
			user.LoginFailedCount++;
			if (user.LoginFailedCount >= LoginOptions.MaxFailedLoginAttempts)
			{
				user.LoginFailedCount = 0;
				user.LockoutEnd = DateTime.Now + LoginOptions.LockoutTimespan;
			}

			UserSet.Update(user);
			await Context.SaveChangesAsync();

			return ServiceResult<bool>.Unauthorized(
				ErrorMessages.Account.LoginFailedInvalid);
		}

		// Still check if email is confirmed no matter what
		// That way, users registered when confirmation was
		// required still need to confirm
		if (!user.EmailConfirmed)
		{
			if (AppOptions.EnableEmail)
			{
				await SendWelcomeEmail(user);
				return ServiceResult<bool>.Unauthorized(
					ErrorMessages.Account.LoginFailedNotConfirmed);
			}

			return ServiceResult<bool>.Unauthorized(
				ErrorMessages.Account.LoginFailedNotConfirmedEmailDisabled);
		}

		// User is authenticated and able to log in
		user.LoginFailedCount = 0;
		UserSet.Update(user);
		await Context.SaveChangesAsync();

		await SignInManager.SignIn(user, login.RememberMe);

		return ServiceResult<bool>.Ok("Logged in successfully");
	}

	/// <inheritdoc />
	public async Task<ServiceResult<bool>> Logout()
	{
		await SignInManager.SignOut();
		return ServiceResult<bool>.Ok("Logged out successfully");
	}

	/// <inheritdoc/>
	public virtual async Task<ServiceResult<bool>> ConfirmAccount(
		ConfirmAccountDto account)
	{
		var user = await UserManager.GetUser(account.UserId);
		if (user is null)
		{
			return ServiceResult<bool>.NotFound(ErrorMessages.Account.AccountErrorInvalidId);
		}

		var status = await VerificationCodeManager.VerifyCode(
			user,
			VerificationCodeTypes.Email,
			account.VerificationCode,
			true);

		if (status == VerificationCodeStatus.Invalid)
		{
			return VerificationCodeInvalid();
		}

		if (status == VerificationCodeStatus.Expired)
		{
			if (AppOptions.EnableEmail)
			{
				await SendWelcomeEmail(user);
				return VerificationCodeExpired();
			}

			return VerificationCodeExpiredEmailDisabled();
		}

		// Code was valid
		user.EmailConfirmed = true;
		UserSet.Update(user);
		await Context.SaveChangesAsync();

		return ServiceResult<bool>.Ok("Account confirmed successfully");
	}

	/// <inheritdoc/>
	public virtual async Task<ServiceResult<bool>> InitiateEmailChange(InitiateEmailChangeDto emailChange)
	{
		var user = await UserManager.GetUser(UserAccessor.GetUserClaimsPrincipal());
		if (user is null)
		{
			return ServiceResult<bool>.Unauthorized(
				ErrorMessages.Account.LoginRequired);
		}

		if (!await UserManager.VerifyPassword(user, emailChange.ConfirmPassword))
		{
			return ServiceResult<bool>.Unauthorized(
				ErrorMessages.Account.LoginFailedInvalid);
		}

		var shouldSendConfirmationEmail = ShouldSendEmailConfirmationEmail();
		if (shouldSendConfirmationEmail)
		{
			user.PendingEmail = emailChange.Email;
		}
		else
		{
			user.Email = emailChange.Email;
		}

		UserSet.Update(user);
		await Context.SaveChangesAsync();

		if (shouldSendConfirmationEmail)
		{
			await SendEmailChangeConfirmationEmail(user);
		}

		return ServiceResult<bool>.Ok("Email change requested");
	}

	/// <inheritdoc/>
	public virtual async Task<ServiceResult<bool>> PerformEmailChange(PerformEmailChangeDto emailChange)
	{
		var user = await UserManager.GetUser(UserAccessor.GetUserClaimsPrincipal());
		if (user is null)
		{
			return ServiceResult<bool>.Unauthorized(
				ErrorMessages.Account.LoginRequired);
		}

		if (user.Id != emailChange.UserId)
		{
			return ServiceResult<bool>.Conflict(ErrorMessages.Account.AccountErrorWrongId);
		}

		var status = await VerificationCodeManager.VerifyCode(
			user,
			VerificationCodeTypes.ChangeEmail,
			emailChange.VerificationCode,
			true);

		if (status == VerificationCodeStatus.Invalid)
		{
			return VerificationCodeInvalid();
		}

		if (status == VerificationCodeStatus.Expired)
		{
			if (AppOptions.EnableEmail)
			{
				await SendEmailChangeConfirmationEmail(user);
				return VerificationCodeExpired();
			}

			return VerificationCodeExpiredEmailDisabled();
		}

		// Code was valid
		user.Email = user.PendingEmail;
		user.PendingEmail = null;
		UserSet.Update(user);
		await Context.SaveChangesAsync();

		return ServiceResult<bool>.Ok("Email changed successfully");
	}

	/// <inheritdoc/>
	public async Task<ServiceResult<bool>> ChangePassword(ChangePasswordDto passwordChange)
	{
		var user = await UserManager.GetUser(UserAccessor.GetUserClaimsPrincipal());
		if (user is null)
		{
			return ServiceResult<bool>.Unauthorized(
				ErrorMessages.Account.LoginRequired);
		}

		if (!await UserManager.VerifyPassword(user, passwordChange.CurrentPassword))
		{
			return ServiceResult<bool>.Unauthorized(
				ErrorMessages.Account.LoginFailedInvalid);
		}

		await UserManager.UpdatePassword(user, passwordChange.NewPassword);

		return ServiceResult<bool>.Ok("Password changed successfully");
	}

	/// <inheritdoc/>
	public async Task<ServiceResult<bool>> ForgotPassword(ForgotPasswordDto passwordRequest)
	{
		BotDetector.DetectSpambot(passwordRequest);

		var user = await UserManager.GetUser(passwordRequest.AccountName);

		// They don't need to know whether the user exists or not
		// so if the user isn't null, send the email
		// otherwise, just return Ok anyway
		if (user != null && AppOptions.EnableEmail)
		{
			await SendPasswordResetEmail(user);
		}

		return ServiceResult<bool>.Ok("Password reset requested");
	}

	/// <inheritdoc/>
	public async Task<ServiceResult<bool>> ResetPassword(ResetPasswordDto resetPassword)
	{
		BotDetector.DetectSpambot(resetPassword);

		var user = await UserManager.GetUser(resetPassword.UserId);
		if (user == null)
		{
			return ServiceResult<bool>.NotFound(ErrorMessages.Account.AccountErrorInvalidId);
		}

		var status = await VerificationCodeManager.VerifyCode(
			user,
			VerificationCodeTypes.PasswordReset,
			resetPassword.VerificationCode,
			true);

		if (status == VerificationCodeStatus.Invalid)
		{
			return VerificationCodeInvalid();
		}

		if (status == VerificationCodeStatus.Expired)
		{
			if (AppOptions.EnableEmail)
			{
				await SendPasswordResetEmail(user);
				return VerificationCodeExpired();
			}

			return VerificationCodeExpiredEmailDisabled();
		}

		// Code was valid
		await UserManager.UpdatePassword(user, resetPassword.NewPassword);
		return ServiceResult<bool>.Ok("Password reset successfully");
	}

	/// <inheritdoc/>
	public async Task<ServiceResult<FileDto>> GetPersonalData()
	{
		var user = await UserManager.GetUser(UserAccessor.GetUserClaimsPrincipal());
		if (user is null)
		{
			return ServiceResult<FileDto>.Unauthorized();
		}

		var file = new FileDto
		{
			Name = "PersonalData.json"
		};
		file.Mime = MimeMapping.MimeUtility.GetMimeMapping(file.Name);

		// Only include personal data for download
		var personalData = new Dictionary<string, string>();
		var personalDataProps = user
			.GetType()
		    .GetProperties()
		    .Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

		foreach (var p in personalDataProps)
		{
			var value = p.GetValue(user)
				?.ToString();
			personalData.Add(p.Name, value ?? "null");
		}

		foreach (var retriever in PersonalDataRetrievers)
		{
			var newData = await retriever.GetUserData(user);
			personalData = personalData.Union(newData)
				.ToDictionary(d => d.Key, d => d.Value);
		}
		// var logins = await _userService.GetLoginsAsync(user);
		// foreach (var l in logins)
		// {
		// 	personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
		// }
		//
		// var authKey = await _userService.GetAuthenticatorKeyAsync(user);
		// if (!string.IsNullOrEmpty(authKey))
		// {
		// 	personalData.Add("Authenticator Key", authKey);
		// }

		file.Contents = JsonSerializer.SerializeToUtf8Bytes(personalData);

		return ServiceResult<FileDto>.Ok(file);
	}

	/// <inheritdoc/>
	public async Task<ServiceResult<bool>> DeleteAccount(DeleteAccountDto deleteAccount)
	{
		var user = await UserManager.GetUser(UserAccessor.GetUserClaimsPrincipal());
		if (user is null)
		{
			return ServiceResult<bool>.Unauthorized(
				ErrorMessages.Account.LoginRequired);
		}

		if (!await UserManager.VerifyPassword(user, deleteAccount.Password))
		{
			return ServiceResult<bool>.Unauthorized(
				ErrorMessages.Account.LoginFailedInvalid);
		}

		UserSet.Remove(user);
		await Context.SaveChangesAsync();

		await SignInManager.SignOut();
		return ServiceResult<bool>.Ok("Account deleted successfully");
	}

	/// <inheritdoc/>
	public async Task<ServiceResult<SienarUserDto>> GetUserInfo()
	{
		var user = await UserManager.GetUser(
			UserAccessor.GetUserClaimsPrincipal(),
			u => u.Roles);
		if (user is null)
		{
			return ServiceResult<SienarUserDto>.Unauthorized(
				ErrorMessages.Account.LoginRequired);
		}

		return ServiceResult<SienarUserDto>.Ok(UserDtoAdapter.MapToDto(user));
	}

	private static ServiceResult<bool> VerificationCodeInvalid()
		=> ServiceResult<bool>.NotFound(
			ErrorMessages.Account.VerificationCodeInvalid);

	private static ServiceResult<bool> VerificationCodeExpired()
		=> ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.VerificationCodeExpired);

	private static ServiceResult<bool> VerificationCodeExpiredEmailDisabled()
		=> ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.VerificationCodeExpiredEmailDisabled);

	private async Task SendWelcomeEmail(TUser user)
	{
		var code = await VerificationCodeManager.CreateCode(
			user,
			VerificationCodeTypes.Email);

		await EmailManager.SendWelcomeEmail(
			user.Username,
			user.Email,
			user.Id,
			code.Code);
	}

	private async Task SendEmailChangeConfirmationEmail(TUser user)
	{
		var code = await VerificationCodeManager.CreateCode(
			user,
			VerificationCodeTypes.ChangeEmail);
		await EmailManager.SendEmailChangeConfirmationEmail(
			user.Username,
			user.PendingEmail,
			user.Id,
			code.Code);
	}

	private async Task SendPasswordResetEmail(TUser user)
	{
		var code = await VerificationCodeManager.CreateCode(
			user,
			VerificationCodeTypes.PasswordReset);
		await EmailManager.SendPasswordResetEmail(
			user.Username,
			user.Email,
			user.Id,
			code.Code);
	}

	private bool ShouldSendEmailConfirmationEmail()
	{
		return LoginOptions.RequireConfirmedAccount && AppOptions.EnableEmail;
	}
}