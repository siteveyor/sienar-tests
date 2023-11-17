using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class ForgotPassword : Base
{
	[Fact]
	public async Task IsSpambot_Throws()
	{
		// Arrange
		var model = new ForgotPasswordDto { SecretKeyField = "spambot" };
		Mocks.BotDetector.SetupDetectSpambotThrows(model);

		// Act
		var action = () => Sut.ForgotPassword(model);

		// Assert
		await action.Should()
			.ThrowAsync<SienarSpambotException>();
	}

	[Fact]
	public async Task UserNotFound_ReturnsOk()
	{
		// Arrange
		var model = new ForgotPasswordDto { AccountName = "nonexistent" };
		var expected = ServiceResult<bool>.Ok();

		// Act
		var result = await Sut.ForgotPassword(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task UserNotFound_DoesNotSendEmail()
	{
		// Arrange
		var model = new ForgotPasswordDto { AccountName = "nonexistent" };
		// Mocks.UserManager.SetupGetUser(model.AccountName, default!);
		// Mocks.VerificationCodeManager.SetupCreateCodePasswordReset();

		// Act
		await Sut.ForgotPassword(model);

		// Assert
		Mocks.VerificationCodeManager.VerifyCreateCodePasswordReset(
			default!,
			Times.Never());
	}

	[Fact]
	public async Task UserFound_ReturnsOk()
	{
		// Arrange
		var model = new ForgotPasswordDto { AccountName = "exists" };
		var expected = ServiceResult<bool>.Ok();

		Mocks.UserManager.SetupGetUser(model.AccountName, new());
		Mocks.VerificationCodeManager.SetupCreateCodePasswordReset();

		// Act
		var result = await Sut.ForgotPassword(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task UserFound_SendsEmail()
	{
		// Arrange
		var model = new ForgotPasswordDto { AccountName = "nonexistent" };
		var user = new TestUser();
		Mocks.UserManager.SetupGetUser(model.AccountName, user);
		Mocks.VerificationCodeManager.SetupCreateCodePasswordReset();

		// Act
		await Sut.ForgotPassword(model);

		// Assert
		Mocks.VerificationCodeManager.VerifyCreateCodePasswordReset(
			user,
			Times.Once());
	}
}