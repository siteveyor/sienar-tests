using System;
using System.Runtime.Serialization;

namespace Sienar.Errors;

public class SienarSpambotException : SienarException
{
	public SienarSpambotException() : base(null) {}

	/// <inheritdoc />
	protected SienarSpambotException(
		SerializationInfo info,
		StreamingContext context)
		: base(info, context) {}

	/// <inheritdoc />
	public SienarSpambotException(string? message)
		: base(message) {}

	/// <inheritdoc />
	public SienarSpambotException(
		string? message,
		Exception? innerException)
		: base(message, innerException) {}
}