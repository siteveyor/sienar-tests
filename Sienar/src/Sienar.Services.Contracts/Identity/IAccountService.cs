using System.Security.Claims;
using System.Threading.Tasks;
using Sienar.Infrastructure;

namespace Sienar.Identity;

public interface IAccountService
{
	/// <summary>
	/// Registers a new user in the database
	/// </summary>
	/// <param name="userData">The data to create the user with</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> Register(RegisterDto userData);

	/// <summary>
	/// Logs a user into the application
	/// </summary>
	/// <param name="login">The user credentials to log in with</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> Login(LoginDto login);

	/// <summary>
	/// Logs out the currently logged in user
	/// </summary>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> Logout();

	/// <summary>
	/// Confirms a new user's account via a unique registration code
	/// </summary>
	/// <param name="account">The information for the user account to confirm</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> ConfirmAccount(ConfirmAccountDto account);

	/// <summary>
	/// Starts the email change process for the currently logged in user account
	/// </summary>
	/// <param name="emailChange">Verification data from the user initiating the change</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> InitiateEmailChange(InitiateEmailChangeDto emailChange);

	/// <summary>
	/// Performs an email change for the currently logged in user account
	/// </summary>
	/// <param name="emailChange">The information for the user account whose email to change</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> PerformEmailChange(PerformEmailChangeDto emailChange);

	/// <summary>
	/// Changes the password for the currently logged in user account
	/// </summary>
	/// <param name="passwordChange">Information verifying the password change request</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> ChangePassword(ChangePasswordDto passwordChange);

	/// <summary>
	/// Initiates the password reset process for the specified user account
	/// </summary>
	/// <param name="passwordRequest">Information to determine which user account to start the password reset process for</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> ForgotPassword(ForgotPasswordDto passwordRequest);

	/// <summary>
	/// Performs the actual password reset for the specified user account
	/// </summary>
	/// <param name="resetPassword">Verification data and new password for the user performing the password reset</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> ResetPassword(ResetPasswordDto resetPassword);

	/// <summary>
	/// Returns the personal data of the currently logged in user, in binary format
	/// </summary>
	/// <returns>a file containing the user's personal data</returns>
	Task<ServiceResult<FileDto>> GetPersonalData();

	/// <summary>
	/// Deletes the account of the currently logged in user
	/// </summary>
	/// <param name="deleteAccount">Information to verify that the user wants to delete their account</param>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<bool>> DeleteAccount(DeleteAccountDto deleteAccount);

	/// <summary>
	/// Gets the account information for the currently logged in user
	/// </summary>
	/// <returns>the result of the operation</returns>
	Task<ServiceResult<SienarUserDto>> GetUserInfo();
}