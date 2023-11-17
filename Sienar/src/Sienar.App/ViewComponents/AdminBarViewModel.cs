using Sienar.Identity;

namespace Sienar.ViewComponents;

public class AdminBarViewModel
{
	public bool IsSignedIn { get; set; }
	public bool IsAdmin { get; set; }
	public bool IsOnDashboard { get; set; }
	public string? Username { get; set; }
}