using System;
using System.Threading.Tasks;

namespace Sienar.Identity;

public interface IVerificationCodeManager<TUser>
{
	Task<VerificationCode> CreateCode(
		TUser user,
		string type);

	Task DeleteCode(
		TUser user,
		string type);

	Task<VerificationCode?> GetCode(
		TUser user,
		string type);

	VerificationCodeStatus GetCodeStatus(
		VerificationCode? code,
		Guid suppliedCode);

	Task<VerificationCodeStatus> GetCodeStatus(
		TUser user,
		string type,
		Guid suppliedCode);

	Task<VerificationCodeStatus> VerifyCode(
		TUser user,
		string type,
		Guid suppliedCode,
		bool deleteIfValid);
}