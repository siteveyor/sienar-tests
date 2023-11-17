using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class InitiateEmailChange : Base
{
	[Fact]
	public async Task UserNotFound_ReturnsUnauthorized()
	{
		// Arrange
		var model = new InitiateEmailChangeDto();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.LoginRequired);

		// Act
		var result = await Sut.InitiateEmailChange(model, new ClaimsPrincipal());

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task IncorrectPassword_ReturnsUnauthorized()
	{
		// Arrange
		var model = new InitiateEmailChangeDto { ConfirmPassword = "password" };
		var user = new TestUser();
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.LoginFailedInvalid);

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.ConfirmPassword, false);

		// Act
		var result = await Sut.InitiateEmailChange(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task Valid_ReturnsOk()
	{
		// Arrange
		var model = new InitiateEmailChangeDto { ConfirmPassword = "password" };
		var user = new TestUser();
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.Ok();

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.ConfirmPassword, true);
		Mocks.VerificationCodeManager.SetupCreateCodeChangeEmail();

		// Act
		var result = await Sut.InitiateEmailChange(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task Valid_SetsPendingEmail()
	{
		// Arrange
		var model = new InitiateEmailChangeDto
		{
			ConfirmPassword = "password",
			Email = "newmail@mail.com"
		};
		var user = new TestUser();
		var claims = new ClaimsPrincipal();

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.ConfirmPassword, true);
		Mocks.VerificationCodeManager.SetupCreateCodeChangeEmail();

		// Act
		await Sut.InitiateEmailChange(model, claims);

		// Assert
		var dbUser = Db.Users.First();

		dbUser.PendingEmail.Should()
			.Be(model.Email);
	}

	[Fact]
	public async Task Valid_SendsEmailChangeConfirmation()
	{
		// Arrange
		var model = new InitiateEmailChangeDto
		{
			ConfirmPassword = "password",
			Email = "newemail@mail.com"
		};
		var user = new TestUser();
		var claims = new ClaimsPrincipal();

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.ConfirmPassword, true);
		Mocks.VerificationCodeManager.SetupCreateCodeChangeEmail();

		// Act
		await Sut.InitiateEmailChange(model, claims);

		// Assert
		Mocks.EmailManager.VerifySendEmailChangeConfirmationEmail(user, Times.Exactly(1));
	}
}