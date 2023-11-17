using System;
using System.Runtime.Serialization;

namespace Sienar.Errors;

public class SienarUnauthorizedException : ApplicationException
{
	public SienarUnauthorizedException()
		: base(ErrorMessages.Generic.NoPermission) {}

	/// <inheritdoc />
	public SienarUnauthorizedException(string? message)
		: base(message) {}

	/// <inheritdoc />
	protected SienarUnauthorizedException(
		SerializationInfo info,
		StreamingContext context)
		: base(info, context) {}

	/// <inheritdoc />
	public SienarUnauthorizedException(
		string? message,
		Exception? innerException)
		: base(message, innerException) {}
}