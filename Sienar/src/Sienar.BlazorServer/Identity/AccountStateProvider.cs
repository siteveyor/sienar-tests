using Sienar.BlazorServer.Tools;

// ReSharper disable once checknamespace
namespace Sienar.Identity;

public class AccountStateProvider : StateProviderBase
{
	private SienarUserDto? _user;

	public SienarUserDto? User
	{
		get => _user;
		set
		{
			if (_user == value)
			{
				return;
			}

			_user = value;
			NotifyStateChanged();
		}
	}
}