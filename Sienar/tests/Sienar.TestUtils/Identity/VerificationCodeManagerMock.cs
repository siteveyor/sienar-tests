using System;
using Moq;

namespace Sienar.Identity;

public class VerificationCodeManagerMock
	: Mock<IVerificationCodeManager<TestUser>>
{

#region Setups

	public VerificationCodeManagerMock SetupCreateCodeEmail(
		VerificationCode? code = null)
		=> SetupCreateCode(
			VerificationCodeTypes.Email,
			code);

	public VerificationCodeManagerMock SetupCreateCodeChangeEmail(
		VerificationCode? code = null)
		=> SetupCreateCode(
			VerificationCodeTypes.ChangeEmail,
			code);

	public VerificationCodeManagerMock SetupCreateCodePasswordReset(
		VerificationCode? code = null)
		=> SetupCreateCode(
			VerificationCodeTypes.PasswordReset,
			code);

	private VerificationCodeManagerMock SetupCreateCode(
		string type,
		VerificationCode? code)
	{
		code ??= new VerificationCode
		{
			Id = Guid.NewGuid(),
			Code = Guid.NewGuid(),
			ConcurrencyStamp = Guid.NewGuid(),
			ExpiresAt = DateTime.Now + TimeSpan.FromDays(1),
			Type = type
		};

		Setup(
				m => m.CreateCode(
					It.IsAny<TestUser>(),
					type))
			.ReturnsAsync(() => code);

		return this;
	}

	public VerificationCodeManagerMock SetupVerifyCodeEmail(
		TestUser user,
		Guid code,
		bool deleteIfValid,
		VerificationCodeStatus result)
		=> SetupVerifyCode(
			user,
			VerificationCodeTypes.Email,
			code,
			deleteIfValid,
			result);

	public VerificationCodeManagerMock SetupVerifyCodeChangeEmail(
		TestUser user,
		Guid code,
		bool deleteIfValid,
		VerificationCodeStatus result)
		=> SetupVerifyCode(
			user,
			VerificationCodeTypes.ChangeEmail,
			code,
			deleteIfValid,
			result);

	public VerificationCodeManagerMock SetupVerifyCodePasswordReset(
		TestUser user,
		Guid code,
		bool deleteIfValid,
		VerificationCodeStatus result)
		=> SetupVerifyCode(
			user,
			VerificationCodeTypes.PasswordReset,
			code,
			deleteIfValid,
			result);

	private VerificationCodeManagerMock SetupVerifyCode(
		TestUser user,
		string type,
		Guid code,
		bool deleteIfValid,
		VerificationCodeStatus result)
	{
		Setup(
				m => m.VerifyCode(
					user,
					type,
					code,
					deleteIfValid))
			.ReturnsAsync(result);

		return this;
	}

#endregion

#region Verifications

	public VerificationCodeManagerMock VerifyCreateCodeEmail(
		TestUser user,
		Times times)
		=> VerifyCreateCode(user, VerificationCodeTypes.Email, times);

	public VerificationCodeManagerMock VerifyCreateCodeChangeEmail(
		TestUser user,
		Times times)
		=> VerifyCreateCode(user, VerificationCodeTypes.ChangeEmail, times);

	public VerificationCodeManagerMock VerifyCreateCodePasswordReset(
		TestUser user,
		Times times)
		=> VerifyCreateCode(user, VerificationCodeTypes.PasswordReset, times);

	private VerificationCodeManagerMock VerifyCreateCode(
		TestUser user,
		string type,
		Times times)
	{
		Verify(m => m.CreateCode(user, type), times);
		return this;
	}

#endregion

}