using Microsoft.Extensions.Logging;
using Sienar.Errors;

namespace Sienar.Infrastructure;

public class BotDetector : IBotDetector
{
	private readonly ILogger<BotDetector> _logger;

	public BotDetector(ILogger<BotDetector> logger)
	{
		_logger = logger;
	}

	/// <inheritdoc />
	public void DetectSpambot(HoneypotDto dto)
	{
		_logger.LogInformation(
			"Form submission completed in {time} seconds",
			dto.TimeToComplete.TotalSeconds);

		if (dto.IsSpambot)
		{
			_logger.LogError(
				"Spambot detected! Value passed to honeypot was '{value}'",
				dto.SecretKeyField);
			throw new SienarSpambotException();
		}
	}
}