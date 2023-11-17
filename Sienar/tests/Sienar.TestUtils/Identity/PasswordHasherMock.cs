using Microsoft.AspNetCore.Identity;
using Moq;

namespace Sienar.Identity;

public class PasswordHasherMock : Mock<IPasswordHasher<TestUser>>
{
	public PasswordHasherMock SetupHashPassword(string hashed = "hashed")
	{
		Setup(
				p => p.HashPassword(
					It.IsAny<TestUser>(),
					It.IsAny<string>()))
			.Returns(hashed);

		return this;
	}

	public PasswordHasherMock SetupVerifyPassword(
		PasswordVerificationResult status = PasswordVerificationResult.Success)
	{
		Setup(
				p => p.VerifyHashedPassword(
					It.IsAny<TestUser>(),
					It.IsAny<string>(),
					It.IsAny<string>()))
			.Returns(status);

		return this;
	}
}