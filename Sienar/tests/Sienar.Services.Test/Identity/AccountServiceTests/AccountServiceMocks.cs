using Microsoft.Extensions.Options;
using Moq;
using Sienar.Infrastructure;

namespace Sienar.Identity.AccountServiceTests;

public class AccountServiceMocks
{
	public VerificationCodeManagerMock VerificationCodeManager { get; } = new();
	public UserManagerMock UserManager { get; } = new();
	public PasswordHasherMock PasswordHasher { get; } = new();
	public AccountEmailManagerMock EmailManager { get; } = new();
	public Mock<IOptions<LoginOptions>> LoginOptions { get; } = new();
	public BotDetectorMock BotDetector { get; } = new();
	public Mock<IUserPersonalDataRetriever<TestUser, TestRole>> PersonalDataRetriever { get; } = new();
}