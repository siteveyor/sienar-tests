using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sienar.Infrastructure;
using Xunit;

namespace Sienar.Identity.AccountServiceTests;

public class GetPersonalData : Base
{
	[Fact]
	public async Task UserNotFound_ReturnsUnauthorized()
	{
		// Arrange
		var claims = new ClaimsPrincipal();
		var expected = ServiceResult<FileDto>.Unauthorized();

		// Act
		var result = await Sut.GetPersonalData(claims);

		// Assert
		result.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public async Task UserFound_ReturnsJsonFile()
	{
		// Arrange
		var claims = new ClaimsPrincipal();
		var user = new TestUser();
		var expectedDto = new FileDto
		{
			Name = "PersonalData.json",
			Mime = MimeMapping.MimeUtility.GetMimeMapping("json")
		};
		var expectedResult = ServiceResult<FileDto>.Ok(expectedDto);

		Mocks.UserManager.SetupGetUser(claims, user);
		SetupPersonalDataRetriever(user);

		// Act
		var result = await Sut.GetPersonalData(claims);

		// Assert
		result.Should()
			.BeEquivalentTo(
				expectedResult,
				o => o.Excluding(d => d.Result!.Contents));
	}

	[Fact]
	public async Task UserFound_FileContainsPersonalData()
	{
		// Arrange
		var claims = new ClaimsPrincipal();
		var user = new TestUser
		{
			Username = "test.username",
			Email = "email@mail.com",
			PendingEmail = "new@mail.com",
			PhoneNumber = "202-555-1234"
		};

		Mocks.UserManager.SetupGetUser(claims, user);
		SetupPersonalDataRetriever(user);

		// Act
		var result = await Sut.GetPersonalData(claims);

		// Assert
		result.Result.Should()
			.NotBeNull();

		var contents = Encoding.UTF8.GetString(
			result.Result!.Contents);
		contents.Should()
			.Contain(nameof(TestUser.Username))
			.And.Contain(user.Username)
			.And.Contain(nameof(TestUser.Email))
			.And.Contain(user.Email)
			.And.Contain(nameof(TestUser.PendingEmail))
			.And.Contain(user.PendingEmail)
			.And.Contain(nameof(TestUser.PhoneNumber))
			.And.Contain(user.PhoneNumber);
	}

	[Fact]
	public async Task UserFound_FileContainsAdditionalPersonalData()
	{
		// Arrange
		var claims = new ClaimsPrincipal();
		var user = new TestUser();
		var key1 = "TestValue1";
		var key2 = "TestValue2";
		var value1 = "12345";
		var value2 = "67890";
		var additionalData = new Dictionary<string, string>
		{
			{ key1, value1 },
			{ key2, value2 }
		};

		Mocks.UserManager.SetupGetUser(claims, user);
		SetupPersonalDataRetriever(user, additionalData);

		// Act
		var result = await Sut.GetPersonalData(claims);

		// Assert
		result.Result.Should()
			.NotBeNull();

		var contents = Encoding.UTF8.GetString(
			result.Result!.Contents);
		contents.Should()
			.Contain(key1)
			.And.Contain(value1)
			.And.Contain(key2)
			.And.Contain(value2);
	}

	private void SetupPersonalDataRetriever(
		TestUser user,
		Dictionary<string, string>? data = null)
	{
		Mocks.PersonalDataRetriever.Setup(
				r => r.GetUserData(user))
			.ReturnsAsync(data ?? new Dictionary<string, string>());
	}
}