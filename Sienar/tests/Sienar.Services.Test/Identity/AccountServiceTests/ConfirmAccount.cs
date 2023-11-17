using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class ConfirmAccount : Base
{
	[Fact]
	public async Task UserNotFound_ReturnsNotFound()
	{
		// Arrange
		var model = new ConfirmAccountDto();
		var expected = ServiceResult<bool>.NotFound(
			ErrorMessages.Account.AccountErrorInvalidId);

		// Act
		var result = await Sut.ConfirmAccount(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task VerificationCodeInvalid_ReturnsNotFound()
	{
		// Arrange
		var model = new ConfirmAccountDto();
		var user = new TestUser();
		var expected = ServiceResult<bool>.NotFound(
			ErrorMessages.Account.VerificationCodeInvalid);

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupVerifyCodeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Invalid);

		// Act
		var result = await Sut.ConfirmAccount(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task VerificationCodeExpired_ReturnsUnauthorized()
	{
		// Arrange
		var model = new ConfirmAccountDto();
		var user = new TestUser();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.VerificationCodeExpired);

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();
		Mocks.VerificationCodeManager.SetupVerifyCodeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Expired);

		// Act
		var result = await Sut.ConfirmAccount(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task VerificationCodeExpired_SendsWelcomeEmail()
	{
		// Arrange
		var model = new ConfirmAccountDto();
		var user = new TestUser();

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();
		Mocks.VerificationCodeManager.SetupVerifyCodeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Expired);

		// Act
		await Sut.ConfirmAccount(model);

		// Assert
		Mocks.VerificationCodeManager.VerifyCreateCodeEmail(user, Times.Exactly(1));
		Mocks.EmailManager.VerifySendWelcomeEmail(user, Times.Exactly(1));
	}

	[Fact]
	public async Task VerificationCodeConfirmed_ReturnsOk()
	{
		// Arrange
		var model = new ConfirmAccountDto();
		var user = new TestUser();
		var expected = ServiceResult<bool>.Ok();

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupVerifyCodeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Valid);

		// Act
		var result = await Sut.ConfirmAccount(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task VerificationCodeConfirmed_SetsEmailConfirmed()
	{
		// Arrange
		var model = new ConfirmAccountDto();
		var user = new TestUser();

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(model.UserId, user);
		Mocks.VerificationCodeManager.SetupVerifyCodeEmail(
			user,
			model.VerificationCode,
			true,
			VerificationCodeStatus.Valid);

		// Act
		await Sut.ConfirmAccount(model);

		// Assert
		var dbUser = Db.Users.First();

		dbUser.EmailConfirmed.Should()
			.BeTrue();
	}
}