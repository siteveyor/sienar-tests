using Moq;
using Sienar.Errors;

namespace Sienar.Infrastructure;

public class BotDetectorMock : Mock<IBotDetector>
{
	public BotDetectorMock SetupDetectSpambotThrows(HoneypotDto dto)
	{
		Setup(s => s.DetectSpambot(dto))
			.Throws<SienarSpambotException>();

		return this;
	}
}