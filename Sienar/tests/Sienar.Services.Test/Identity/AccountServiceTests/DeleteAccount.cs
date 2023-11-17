using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class DeleteAccount : Base
{
	[Fact]
	public async Task UserNotFound_ReturnsUnauthorized()
	{
		// Arrange
		var model = new DeleteAccountDto();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.LoginRequired);

		// Act
		var result = await Sut.DeleteAccount(model, new ClaimsPrincipal());

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task PasswordIncorrect_ReturnsUnauthorized()
	{
		// Arrange
		var model = new DeleteAccountDto { Password = "incorrect" };
		var user = new TestUser();
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.Unauthorized(
			ErrorMessages.Account.LoginFailedInvalid);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, false);

		// Act
		var result = await Sut.DeleteAccount(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task PasswordCorrect_ReturnsOk()
	{
		// Arrange
		var model = new DeleteAccountDto { Password = "correct" };
		var user = new TestUser();
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<bool>.Ok();

		Db.AddUser(user);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, true);

		// Act
		var result = await Sut.DeleteAccount(model, claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task PasswordCorrect_DeletesUser()
	{
		// Arrange
		var model = new DeleteAccountDto { Password = "correct" };
		var user = new TestUser();
		var claims = new ClaimsPrincipal();

		Db.AddUser(user);

		Db.Users.Should()
			.HaveCount(1);

		Mocks.UserManager.SetupGetUser(claims, user);
		Mocks.UserManager.SetupVerifyPassword(user, model.Password, true);

		// Act
		await Sut.DeleteAccount(model, claims);

		// Assert
		Db.Users.Should()
			.HaveCount(0);
	}
}