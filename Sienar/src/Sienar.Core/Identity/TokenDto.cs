namespace Sienar.Identity;

public class TokenDto
{
	public string Token { get; set; }

	public TokenDto(string token)
	{
		Token = token;
	}
}