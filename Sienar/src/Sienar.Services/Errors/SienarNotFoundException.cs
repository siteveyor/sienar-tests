using System;
using System.Runtime.Serialization;

namespace Sienar.Errors;

public class SienarNotFoundException : ApplicationException
{
	public SienarNotFoundException()
		: base(ErrorMessages.Generic.NotFound) {}

	/// <inheritdoc />
	public SienarNotFoundException(string? message)
		: base(message) {}

	/// <inheritdoc />
	protected SienarNotFoundException(
		SerializationInfo info,
		StreamingContext context)
		: base(info, context) {}

	/// <inheritdoc />
	public SienarNotFoundException(
		string message,
		Exception? innerException)
		: base(message, innerException) {}
}