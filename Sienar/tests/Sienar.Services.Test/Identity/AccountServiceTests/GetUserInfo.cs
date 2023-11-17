using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Sienar.Errors;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class GetUserInfo : Base
{
	[Fact]
	public async Task UserNotFound_ReturnsUnauthorized()
	{
		// Arrange
		var expected = ServiceResult<SienarUserDto>.Unauthorized(
			ErrorMessages.Account.LoginRequired);

		// Act
		var result = await Sut.GetUserInfo(new ClaimsPrincipal());

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task UserFound_ReturnsUser()
	{
		// Arrange
		var user = new TestUser
		{
			Roles = new()
			{
				new TestRole { Name = "role1" }
			}
		};
		var dto = Adapter.MapToDto(user);
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<SienarUserDto>.Ok(dto);

		Mocks.UserManager.SetupGetUser(claims, user);

		// Act
		var result = await Sut.GetUserInfo(claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}
}