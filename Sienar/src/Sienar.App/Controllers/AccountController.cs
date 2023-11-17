using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sienar.Identity;

namespace Sienar.Controllers;

[ApiController]
[Route("sienar/account")]
[Authorize]
public class AccountController
	: SienarControllerBase<AccountController, IAccountService>
{
	/// <inheritdoc />
	public AccountController(
		ILogger<AccountController> logger,
		IAccountService service)
		: base(logger, service) {}

	[AllowAnonymous]
	[HttpPost]
	public Task<IActionResult> Register(RegisterDto userData)
		=> ProcessServiceCall(() => Service.Register(userData));

	[HttpGet("login")]
	public Task<IActionResult> GetUserData()
		=> ProcessServiceCall(() => Service.GetUserInfo());

	[AllowAnonymous]
	[HttpPost("login")]
	public Task<IActionResult> Login(LoginDto login) 
		=> ProcessServiceCall(() => Service.Login(login));

	[HttpDelete("login")]
	public Task<IActionResult> Logout()
		=> ProcessServiceCall(() => Service.Logout());

	[AllowAnonymous]
	[HttpPost("confirm")]
	public Task<IActionResult> ConfirmAccount(ConfirmAccountDto accountDto)
		=> ProcessServiceCall(() => Service.ConfirmAccount(accountDto));

	[HttpPost("email")]
	public Task<IActionResult> InitiateEmailChange(InitiateEmailChangeDto emailDto)
		=> ProcessServiceCall(() => Service.InitiateEmailChange(emailDto));

	[HttpPatch("email")]
	public Task<IActionResult> PerformEmailChange(PerformEmailChangeDto changeDto)
		=> ProcessServiceCall(() => Service.PerformEmailChange(changeDto));

	[HttpPost("password")]
	public Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
		=> ProcessServiceCall(() => Service.ChangePassword(passwordDto));

	[AllowAnonymous]
	[HttpDelete("password")]
	public Task<IActionResult> ForgotPassword(ForgotPasswordDto passwordDto)
		=> ProcessServiceCall(
			() => Service.ForgotPassword(passwordDto),
			HttpStatusCode.Accepted);

	[AllowAnonymous]
	[HttpPatch("password")]
	public Task<IActionResult> ResetPassword(ResetPasswordDto passwordDto)
		=> ProcessServiceCall(() => Service.ResetPassword(passwordDto));

	[HttpGet("data")]
	public Task<IActionResult> GetPersonalData()
		=> ProcessServiceCallReturningFile(() => Service.GetPersonalData());

	[HttpDelete]
	public Task<IActionResult> DeleteAccount(DeleteAccountDto deleteAccountDto)
		=> ProcessServiceCall(() => Service.DeleteAccount(deleteAccountDto));

	[AllowAnonymous]
	[HttpGet("initialize")]
	public async Task<IActionResult> InitializeSession([FromServices] IAntiforgery antiforgeryService)
	{
		// This action doesn't simply wrap a service call
		// because a good portion of this code
		// is an application-level concern
		var sessionInfo = new SessionInfoDto
		{
			Token = antiforgeryService.GetAndStoreTokens(HttpContext)
				.RequestToken!
		}; 

		var userInfoResult = await Service.GetUserInfo();
		if (userInfoResult.WasSuccessful)
		{
			sessionInfo.User = userInfoResult.Result;
		}

		return Ok(sessionInfo);
	}
}