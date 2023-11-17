using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Moq;

namespace Sienar.Identity;

public class UserManagerMock : Mock<IUserManager<TestUser>>
{
	public UserManagerMock SetupGetUser(string username, TestUser user)
	{
		Setup(m => m.GetUser(
				username,
				It.IsAny<Func<IQueryable<TestUser>, IQueryable<TestUser>>>()))
			.ReturnsAsync(user);

		return this;
	}

	public UserManagerMock SetupGetUser(Guid userId, TestUser user)
	{
		Setup(m => m.GetUser(
				userId,
				It.IsAny<Func<IQueryable<TestUser>, IQueryable<TestUser>>>()))
			.ReturnsAsync(user);

		return this;
	}

	public UserManagerMock SetupGetUser(ClaimsPrincipal claims, TestUser user)
	{
		Setup(m => m.GetUser(
				claims,
				It.IsAny<Func<IQueryable<TestUser>, IQueryable<TestUser>>>()))
			.ReturnsAsync(user);

		return this;
	}

	public UserManagerMock SetupVerifyPassword(
		TestUser user,
		string password,
		bool isSuccessful)
	{
		Setup(m => m.VerifyPassword(user, password))
			.ReturnsAsync(() => isSuccessful);

		return this;
	}

	public UserManagerMock VerifyUpdatePassword(
		TestUser user,
		string password,
		Times times)
	{
		Verify(
			m => m.UpdatePassword(user, password),
			times);

		return this;
	}
}