namespace Sienar.Infrastructure;

public interface IBotDetector
{
	/// <summary>
	/// Determines whether a honeypot-enabled DTO has caught a bot
	/// </summary>
	/// <param name="dto">The DTO to test for bots</param>
	/// <returns>whether the DTO </returns>
	void DetectSpambot(HoneypotDto dto);
}