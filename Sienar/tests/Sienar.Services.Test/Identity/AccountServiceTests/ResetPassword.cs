using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class ResetPassword : Base
{
	[Fact]
	public async Task IsSpambot_Throws()
	{
		// Arrange
		var model = new ResetPasswordDto { SecretKeyField = "spambot" };
		Mocks.BotDetector.SetupDetectSpambotThrows(model);

		// Act
		var action = () => Sut.ResetPassword(model);

		// Assert
		await action.Should()
			.ThrowAsync<SienarSpambotException>();
	}

	[Fact]
	public async Task UserNotFound_ReturnsNotFound()
	{
		// Arrange
		var model = new ResetPasswordDto();
		var expected = ServiceResult<bool>.NotFound(
			ErrorMessages.Account.AccountErrorInvalidId);

		// Act
		var result = await Sut.ResetPassword(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task CodeInvalid_ReturnsNotFound()
	{
		// Arrange
		var model = new ResetPasswordDto
		{
			UserId = Guid.NewGuid(),
			VerificationCode = Guid.NewGuid()
		};
		var user = new TestUser { Id = model.UserId };
		var expected = ServiceResult<bool>.NotFound(
			ErrorMessages.Account.VerificationCodeInvalid);

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupVerifyCodePasswordReset(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Invalid);

		// Act
		var result = await Sut.ResetPassword(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task CodeExpired_ReturnsUnauthorized()
	{
		// Arrange
		var model = new ResetPasswordDto
		{
			UserId = Guid.NewGuid(),
			VerificationCode = Guid.NewGuid()
		};
		var user = new TestUser { Id = model.UserId };
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.VerificationCodeExpired);

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupVerifyCodePasswordReset(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Expired);
		Mocks.VerificationCodeManager.SetupCreateCodePasswordReset();

		// Act
		var result = await Sut.ResetPassword(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task CodeExpired_SendsEmail()
	{
		// Arrange
		var model = new ResetPasswordDto
		{
			UserId = Guid.NewGuid(),
			VerificationCode = Guid.NewGuid()
		};
		var user = new TestUser { Id = model.UserId };

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupVerifyCodePasswordReset(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Expired);
		Mocks.VerificationCodeManager.SetupCreateCodePasswordReset();

		// Act
		await Sut.ResetPassword(model);

		// Assert
		Mocks.EmailManager.VerifySendPasswordResetEmail(user, Times.Once());
	}

	[Fact]
	public async Task CodeValid_ReturnsOk()
	{
		// Arrange
		var model = new ResetPasswordDto
		{
			UserId = Guid.NewGuid(),
			VerificationCode = Guid.NewGuid()
		};
		var user = new TestUser { Id = model.UserId };
		var expected = ServiceResult<bool>.Ok();

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupVerifyCodePasswordReset(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Valid);

		// Act
		var result = await Sut.ResetPassword(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task CodeValid_UpdatesPassword()
	{
		// Arrange
		var model = new ResetPasswordDto
		{
			UserId = Guid.NewGuid(),
			VerificationCode = Guid.NewGuid(),
			NewPassword = "new password"
		};
		var user = new TestUser { Id = model.UserId };

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupVerifyCodePasswordReset(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Valid);

		// Act
		await Sut.ResetPassword(model);

		// Assert
		Mocks.UserManager.VerifyUpdatePassword(
			user,
			model.NewPassword,
			Times.Once());
	}
}