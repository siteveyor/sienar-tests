using System;
using System.Runtime.Serialization;

namespace Sienar.Errors;

public class SienarConflictException : SienarException
{
	public SienarConflictException()
		: base(ErrorMessages.Generic.DataConflict) {}

	/// <inheritdoc />
	public SienarConflictException(string? message)
		: base(message) {}

	/// <inheritdoc />
	protected SienarConflictException(
		SerializationInfo info,
		StreamingContext context)
		: base(info, context) {}

	/// <inheritdoc />
	public SienarConflictException(
		string? message,
		Exception? innerException)
		: base(message, innerException) {}
}