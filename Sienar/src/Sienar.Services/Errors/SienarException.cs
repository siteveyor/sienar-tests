using System;
using System.Runtime.Serialization;

namespace Sienar.Errors;

public class SienarException : Exception
{
	/// <inheritdoc />
	public SienarException(string? message)
		: base(message) {}

	/// <inheritdoc />
	protected SienarException(
		SerializationInfo info,
		StreamingContext context)
		: base(info, context) {}

	/// <inheritdoc />
	public SienarException(
		string? message,
		Exception? innerException)
		: base(message, innerException) {}
}