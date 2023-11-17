using System;
using System.Linq;
using Moq;

namespace Sienar.Identity;

public class AccountEmailManagerMock : Mock<IAccountEmailManager>
{
	public AccountEmailManagerMock VerifySendWelcomeEmail(
		TestUser user,
		Times times)
	{
		Verify(
			m => m.SendWelcomeEmail(
				user.Username,
				user.Email,
				user.Id,
				It.IsAny<Guid>()),
			times);

		return this;
	}

	public AccountEmailManagerMock VerifySendEmailChangeConfirmationEmail(
		TestUser user,
		Times times)
	{
		Verify(
			m => m.SendEmailChangeConfirmationEmail(
				user.Username,
				user.PendingEmail,
				user.Id,
				It.IsAny<Guid>()),
			times);

		return this;
	}

	public AccountEmailManagerMock VerifySendPasswordResetEmail(
		TestUser user,
		Times times)
	{
		Verify(
			m => m.SendPasswordResetEmail(
				user.Username,
				user.Email,
				user.Id,
				It.IsAny<Guid>()),
			times);

		return this;
	}
}