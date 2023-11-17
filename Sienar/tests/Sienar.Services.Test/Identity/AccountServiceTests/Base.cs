namespace Sienar.Identity.AccountServiceTests;

public class Base : UnitTestWithDbContextBase<
	AccountService<
		TestUser,
		SienarUserDto,
		TestRole,
		UserDtoAdapter<TestUser, SienarUserDto, TestRole>,
		TestDbContext>,
	AccountServiceMocks>
{
	protected readonly LoginOptions LoginOptions = new ();
	protected readonly UserDtoAdapter<TestUser, SienarUserDto, TestRole> Adapter;

	/// <inheritdoc />
	public Base()
	{
		Adapter = new();
		Mocks.LoginOptions.Setup(o => o.Value)
			.Returns(LoginOptions);

		Sut = new(
			Db,
			Adapter,
			Mocks.VerificationCodeManager.Object,
			Mocks.UserManager.Object,
			Mocks.PasswordHasher.Object,
			Mocks.EmailManager.Object,
			Mocks.LoginOptions.Object,
			Mocks.BotDetector.Object,
			new [] { Mocks.PersonalDataRetriever.Object });
	}
}