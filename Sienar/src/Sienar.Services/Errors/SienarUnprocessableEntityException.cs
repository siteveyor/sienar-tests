using System;
using System.Runtime.Serialization;

namespace Sienar.Errors;

public class SienarUnprocessableEntityException : ApplicationException
{
	public SienarUnprocessableEntityException()
		: base(ErrorMessages.Generic.Unprocessable) {}

	/// <inheritdoc />
	public SienarUnprocessableEntityException(string? message)
		: base(message) {}

	/// <inheritdoc />
	protected SienarUnprocessableEntityException(
		SerializationInfo info,
		StreamingContext context)
		: base(info, context) {}

	/// <inheritdoc />
	public SienarUnprocessableEntityException(
		string? message,
		Exception? innerException)
		: base(message, innerException) {}
}