using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class ChangePassword : Base
{
	[Fact]
	public async Task UserNotFound_ReturnsUnauthorized()
	{
		// Arrange
		var model = new ChangePasswordDto();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.LoginRequired);

		// Act
		var result = await Sut.ChangePassword(model, new ClaimsPrincipal());

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}
	[Fact]
	public async Task IncorrectPassword_ReturnsUnauthorized()
	{
		// Arrange
		var model = new ChangePasswordDto { CurrentPassword = "incorrect_password" };
		var user = new TestUser();
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.LoginFailedInvalid);

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(
			user,
			model.CurrentPassword,
			false);

		// Act
		var result = await Sut.ChangePassword(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task CorrectPassword_ReturnsOk()
	{
		// Arrange
		var model = new ChangePasswordDto
		{
			CurrentPassword = "correct_password",
			NewPassword = "new_password"
		};
		var user = new TestUser();
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.Ok();

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(
			user,
			model.CurrentPassword,
			true);

		// Act
		var result = await Sut.ChangePassword(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task CorrectPassword_UpdatesPassword()
	{
		// Arrange
		var model = new ChangePasswordDto
		{
			CurrentPassword = "correct_password",
			NewPassword = "new_password"
		};
		var user = new TestUser();
		var claims = new ClaimsPrincipal();

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(
			user,
			model.CurrentPassword,
			true);

		// Act
		await Sut.ChangePassword(model, claims);

		// Assert
		Mocks.UserManager.VerifyUpdatePassword(
			user,
			model.NewPassword,
			Times.Once());
	}
}