using System.Text.Json;

namespace Sienar.Infrastructure;

public class SienarJsonSerializer : IJsonSerializer
{
	private readonly JsonSerializerOptions _options = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	/// <inheritdoc />
	public string Serialize(object value) => JsonSerializer.Serialize(value, _options);
}