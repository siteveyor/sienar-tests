using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class PerformEmailChange : Base
{
	[Fact]
	public async Task UserNotFound_ReturnsUnauthorized()
	{
		// Arrange
		var model = new PerformEmailChangeDto();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.LoginRequired);

		// Act
		var result = await Sut.PerformEmailChange(model, new ClaimsPrincipal());

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task UserIdMismatch_ReturnsConflict()
	{
		// Arrange
		var model = new PerformEmailChangeDto { UserId = Guid.NewGuid() };
		var user = new TestUser();
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.Conflict(ErrorMessages.Account.AccountErrorWrongId);

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(claims, user);

		// Act
		var result = await Sut.PerformEmailChange(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task VerificationCodeInvalid_ReturnsNotFound()
	{
		// Arrange
		var user = new TestUser();
		Db.AddUser(user);

		var model = new PerformEmailChangeDto { UserId = user.Id };
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.NotFound(
			ErrorMessages.Account.VerificationCodeInvalid);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.VerificationCodeManager.SetupVerifyCodeChangeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Invalid);

		// Act
		var result = await Sut.PerformEmailChange(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
		
	}

	[Fact]
	public async Task VerificationCodeExpired_ReturnsUnauthorized()
	{
		// Arrange
		var user = new TestUser();
		Db.AddUser(user);

		var model = new PerformEmailChangeDto { UserId = user.Id };
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.VerificationCodeExpired);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.VerificationCodeManager.SetupVerifyCodeChangeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Expired);
		Mocks.VerificationCodeManager.SetupCreateCodeChangeEmail();

		// Act
		var result = await Sut.PerformEmailChange(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
		
	}

	[Fact]
	public async Task VerificationCodeExpired_ResendsEmailChangeConfirmationEmail()
	{
		// Arrange
		var user = new TestUser();
		Db.AddUser(user);

		var model = new PerformEmailChangeDto { UserId = user.Id };
		var claims = new ClaimsPrincipal();

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.VerificationCodeManager.SetupVerifyCodeChangeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Expired);
		Mocks.VerificationCodeManager.SetupCreateCodeChangeEmail();

		// Act
		await Sut.PerformEmailChange(model, claims);

		// Assert
		Mocks.VerificationCodeManager.VerifyCreateCodeChangeEmail(user, Times.Exactly(1));
		Mocks.EmailManager.VerifySendEmailChangeConfirmationEmail(user, Times.Exactly(1));
	}

	[Fact]
	public async Task Valid_ReturnsOk()
	{
		// Arrange
		var user = new TestUser{ PendingEmail = "new@mail.com" };
		Db.AddUser(user);

		var model = new PerformEmailChangeDto
		{
			UserId = user.Id,
		};
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.Ok();

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.VerificationCodeManager.SetupVerifyCodeChangeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Valid);
		Mocks.VerificationCodeManager.SetupCreateCodeChangeEmail();

		// Act
		var result = await Sut.PerformEmailChange(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task Valid_UpdatesEmail()
	{
		// Arrange
		var newEmail = "new@mail.com";
		var user = new TestUser{ PendingEmail = newEmail };
		Db.AddUser(user);

		var model = new PerformEmailChangeDto
		{
			UserId = user.Id,
		};
		var claims = new ClaimsPrincipal();

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.VerificationCodeManager.SetupVerifyCodeChangeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Valid);
		Mocks.VerificationCodeManager.SetupCreateCodeChangeEmail();

		// Act
		await Sut.PerformEmailChange(model, claims);

		// Assert
		var dbUser = Db.Users.First();

		dbUser.PendingEmail.Should()
			.BeNull();
		dbUser.Email.Should()
			.Be(newEmail);
	}
}