using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class Login : Base
{
	[Fact]
	public async Task IsSpambot_Throws()
	{
		// Arrange
		var model = new LoginDto { SecretKeyField = "spambot" };
		Mocks.BotDetector.SetupDetectSpambotThrows(model);

		// Act
		var action = () => Sut.Login(model);

		// Assert
		await action.Should()
			.ThrowAsync<SienarSpambotException>();
	}

	[Fact]
	public async Task UserNotFound_ReturnsNotFound()
	{
		// Arrange
		var model = new LoginDto { AccountName = "nonexistent" };
		var expected = ServiceResult<bool>.NotFound(
			ErrorMessages.Account.LoginFailedNotFound);

		// Act
		var result = await Sut.Login(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task UserLockedOut_ReturnsUnauthorized()
	{
		// Arrange
		var model = new LoginDto { AccountName = "username" };
		var user = new TestUser
		{
			LockoutEnd = DateTime.Now.AddHours(1)
		};
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.GetLockoutMessage(user.LockoutEnd));

		Mocks.UserManager.SetupGetUser(model.AccountName, user);

		// Act
		var result = await Sut.Login(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task IncorrectPassword_ReturnsUnauthorized()
	{
		// Arrange
		var model = new LoginDto
		{
			AccountName = "username",
			Password = "password"
		};
		var user = new TestUser();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.LoginFailedInvalid);

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(model.AccountName, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, false);

		// Act
		var result = await Sut.Login(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task IncorrectPassword_IncrementsLoginFailedCount()
	{
		// Arrange
		var model = new LoginDto
		{
			AccountName = "username",
			Password = "password"
		};
		var user = new TestUser();
		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(model.AccountName, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, false);

		// Act
		await Sut.Login(model);

		// Assert
		var dbUser = Db.Users.First();

		dbUser.LoginFailedCount.Should()
			.Be(1);
	}

	[Fact]
	public async Task MaxLoginAttemptsExceeded_LocksOutUser()
	{
		// Arrange
		var model = new LoginDto
		{
			AccountName = "username",
			Password = "password"
		};
		var user = new TestUser
		{
			LoginFailedCount = LoginOptions.MaxFailedLoginAttempts - 1
		};
		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(model.AccountName, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, false);

		// Act
		await Sut.Login(model);

		// Assert
		var dbUser = Db.Users.First();

		dbUser.LoginFailedCount.Should()
			.Be(0);
		dbUser.LockoutEnd!.Value
			.Should()
			.BeCloseTo(
				DateTime.Now + LoginOptions.LockoutTimespan,
				TimeSpan.FromSeconds(1));
	}

	[Fact]
	public async Task EmailNotConfirmed_ReturnsUnauthorized()
	{
		// Arrange
		var model = new LoginDto
		{
			AccountName = "username",
			Password = "password"
		};
		var user = new TestUser { EmailConfirmed = false };
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.LoginFailedNotConfirmed);

		Mocks.UserManager.SetupGetUser(model.AccountName, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, true);
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();

		// Act
		var result = await Sut.Login(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task EmailNotConfirmed_SendsWelcomeEmail()
	{
		// Arrange
		var model = new LoginDto
		{
			AccountName = "username",
			Password = "password"
		};
		var user = new TestUser { EmailConfirmed = false };

		Mocks.UserManager.SetupGetUser(model.AccountName, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, true);
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();

		// Act
		await Sut.Login(model);

		// Assert
		Mocks.VerificationCodeManager.VerifyCreateCodeEmail(user, Times.Exactly(1));
		Mocks.EmailManager.VerifySendWelcomeEmail(user, Times.Exactly(1));
	}

	[Fact]
	public async Task EmailConfirmed_DoesNotSendWelcomeEmail()
	{
		// Arrange
		var model = new LoginDto
		{
			AccountName = "username",
			Password = "password"
		};
		var user = new TestUser { EmailConfirmed = true };
		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(model.AccountName, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, true);
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();

		// Act
		await Sut.Login(model);

		// Assert
		Mocks.VerificationCodeManager.VerifyCreateCodeEmail(user, Times.Never());
		Mocks.EmailManager.VerifySendWelcomeEmail(user, Times.Never());
	}

	[Fact]
	public async Task Valid_ReturnsOk()
	{
		// Arrange
		var model = new LoginDto
		{
			AccountName = "username",
			Password = "password"
		};
		var user = new TestUser { EmailConfirmed = true };
		var expected = ServiceResult<bool>.Ok();
		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(model.AccountName, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, true);
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();

		// Act
		var result = await Sut.Login(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task Valid_ResetsLoginFailedCount()
	{
		// Arrange
		var model = new LoginDto
		{
			AccountName = "username",
			Password = "password"
		};
		var user = new TestUser
		{
			EmailConfirmed = true,
			LoginFailedCount = 1
		};
		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(model.AccountName, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, true);
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();

		// Act
		await Sut.Login(model);

		// Assert
		var dbUser = Db.Users.First();

		dbUser.LoginFailedCount.Should()
			.Be(0);
	}
}