namespace Sienar.Identity;

public class SessionInfoDto
{
	public string Token { get; set; } = default!;
	public SienarUserDto? User { get; set; }
}