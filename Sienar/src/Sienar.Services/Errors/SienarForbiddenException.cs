using System;
using System.Runtime.Serialization;

namespace Sienar.Errors;

public class SienarForbiddenException : SienarException
{
	public SienarForbiddenException()
		: base(ErrorMessages.Generic.NoPermission) {}

	/// <inheritdoc />
	public SienarForbiddenException(string? message)
		: base(message) {}

	/// <inheritdoc />
	protected SienarForbiddenException(
		SerializationInfo info,
		StreamingContext context)
		: base(info, context) {}

	/// <inheritdoc />
	public SienarForbiddenException(
		string? message,
		Exception? innerException)
		: base(message, innerException) {}
}