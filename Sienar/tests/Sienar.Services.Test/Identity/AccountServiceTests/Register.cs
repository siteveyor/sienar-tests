using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class Register : Base
{
	private readonly RegisterDto _passingRegisterDto = new()
	{
		Username = "user",
		Email = "email@mail.com",
		Password = "password",
		ConfirmPassword = "password",
		AcceptTos = true
	};

	[Fact]
	public async Task IsSpambot_Throws()
	{
		// Arrange
		var model = new RegisterDto { SecretKeyField = "spambot" };
		Mocks.BotDetector.SetupDetectSpambotThrows(model);

		// Act
		var action = () => Sut.Register(model);

		// Assert
		await action.Should()
			.ThrowAsync<SienarSpambotException>();
	}

	[Fact]
	public async Task NotAcceptTos_ReturnsUnprocessable()
	{
		// Arrange
		var model = new RegisterDto { AcceptTos = false};
		var expected = ServiceResult<bool>.Unprocessable(
			ErrorMessages.Account.MustAcceptTos);

		// Act
		var result = await Sut.Register(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task UsernameTaken_ReturnsUnprocessable()
	{
		// Arrange
		var username = "taken";
		var model = new RegisterDto
		{
			AcceptTos = true,
			Username = username
		};
		var expected = ServiceResult<bool>.Unprocessable(
			ErrorMessages.Account.UsernameTaken);

		Db.AddUser(new() { Username = username });

		// Act
		var result = await Sut.Register(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task EmailTakenConfirmed_ReturnsUnprocessable()
	{
		// Arrange
		var email = "taken@mail.com";
		var model = new RegisterDto
		{
			AcceptTos = true,
			Username = "nottaken1",
			Email = email
		};
		var expected = ServiceResult<bool>.Unprocessable(
			ErrorMessages.Account.EmailTaken);

		var user = new TestUser
		{
			Username = "nottaken2",
			Email = email
		};
		Db.AddUser(user);

		// Act
		var result = await Sut.Register(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task EmailTakenPending_ReturnsUnprocessable()
	{
		// Arrange
		var email = "taken@mail.com";
		var model = new RegisterDto
		{
			AcceptTos = true,
			Username = "nottaken1",
			Email = email
		};
		var expected = ServiceResult<bool>.Unprocessable(
			ErrorMessages.Account.EmailTaken);

		var user = new TestUser
		{
			Username = "nottaken2",
			Email = "nottaken@mail.com",
			PendingEmail = email
		};
		Db.AddUser(user);

		// Act
		var result = await Sut.Register(model);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task RequirementsMet_ReturnsOk()
	{
		// Arrange
		var code = new VerificationCode { Code = Guid.NewGuid() };
		var expected = ServiceResult<bool>.Ok();
		Mocks.VerificationCodeManager.SetupCreateCodeEmail(code);

		// Act
		var result = await Sut.Register(_passingRegisterDto);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task RequirementsMet_AddsUser()
	{
		// Arrange
		var password = "hashedpw";
		Mocks.PasswordHasher.SetupHashPassword(password);
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();
		var expected = new TestUser
		{
			Username = _passingRegisterDto.Username,
			Email = _passingRegisterDto.Email,
			PasswordHash = password
		};

		// Act
		await Sut.Register(_passingRegisterDto);

		// Assert
		Db.Users.Should()
			.HaveCount(1);

		var user = Db.Users.First();

		user.Should()
			.BeEquivalentTo(
				expected,
				o => o.Including(u => u.Username)
						.Including(u => u.Email)
						.Including(u => u.PasswordHash));
		user.ConcurrencyStamp.Should()
			.NotBe(Guid.Empty);
	}

	[Fact]
	public async Task DoesntRequireConfirmedAccount_EmailConfirmedByDefault()
	{
		// Arrange
		LoginOptions.RequireConfirmedAccount = false;
		Mocks.PasswordHasher.SetupHashPassword();
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();

		// Act
		await Sut.Register(_passingRegisterDto);

		// Assert
		var user = Db.Users.First();

		user.EmailConfirmed.Should()
			.BeTrue();
	}

	[Fact]
	public async Task DoesntRequireConfirmedAccount_DoesntSendWelcomeEmail()
	{
		// Arrange
		LoginOptions.RequireConfirmedAccount = false;
		Mocks.PasswordHasher.SetupHashPassword();
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();

		// Act
		await Sut.Register(_passingRegisterDto);

		// Assert
		var user = Db.Users.First();

		Mocks.VerificationCodeManager.VerifyCreateCodeEmail(user, Times.Never());
		Mocks.EmailManager.VerifySendWelcomeEmail(user, Times.Never());
	}

	[Fact]
	public async Task RequiresConfirmedAccount_EmailNotConfirmedByDefault()
	{
		// Arrange
		LoginOptions.RequireConfirmedAccount = true;
		Mocks.PasswordHasher.SetupHashPassword();
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();

		// Act
		await Sut.Register(_passingRegisterDto);

		// Assert
		var user = Db.Users.First();

		user.EmailConfirmed.Should()
			.BeFalse();
	}

	[Fact]
	public async Task RequiresConfirmedAccount_SendsWelcomeEmail()
	{
		// Arrange
		LoginOptions.RequireConfirmedAccount = true;
		Mocks.PasswordHasher.SetupHashPassword();
		Mocks.VerificationCodeManager.SetupCreateCodeEmail();

		// Act
		await Sut.Register(_passingRegisterDto);

		// Assert
		var user = Db.Users.First();

		Mocks.VerificationCodeManager.VerifyCreateCodeEmail(user, Times.Exactly(1));
		Mocks.EmailManager.VerifySendWelcomeEmail(user, Times.Exactly(1));
	}
}