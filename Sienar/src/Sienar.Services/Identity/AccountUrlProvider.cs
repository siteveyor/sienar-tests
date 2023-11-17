using Microsoft.Extensions.Options;
using Sienar.Email;

namespace Sienar.Identity;

public class AccountUrlProvider : IAccountUrlProvider
{
	protected readonly EmailOptions Options;

	public AccountUrlProvider(IOptions<EmailOptions> options)
	{
		Options = options.Value;
	}

	/// <inheritdoc />
	public virtual string ConfirmationUrl
		=> $"{Options.ApplicationUrl}/Account/Confirm";

	/// <inheritdoc />
	public virtual string EmailChangeUrl
		=> $"{Options.ApplicationUrl}{Options.DashboardUrl}/Account/Email/Confirm";

	/// <inheritdoc />
	public virtual string ResetPasswordUrl
		=> $"{Options.ApplicationUrl}/Account/ResetPassword";
}