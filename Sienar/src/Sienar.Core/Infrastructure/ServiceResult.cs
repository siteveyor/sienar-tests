namespace Sienar.Infrastructure;

public class ServiceResult<T>
{
	/// <summary>
	/// Creates a successful <see cref="ServiceResult{T}"/>
	/// </summary>
	/// <param name="message">The success message</param>
	/// <param name="messageType">The <see cref="MessageType"/> of the message</param>
	/// <returns>a <see cref="ServiceResult{T}"/> that indicates a service call was successful</returns>
	public static ServiceResult<T> Ok(
		string? message = null,
		MessageType messageType = MessageType.Success) => new ()
	{
		ServiceError = ServiceError.None,
		Message = message,
		MessageType = messageType
	};

	/// <summary>
	/// Creates a successful <see cref="ServiceResult{T}"/> that includes a result payload
	/// </summary>
	/// <param name="result">The result payload to return from the service call</param>
	/// <param name="message">The success message</param>
	/// <param name="messageType">The <see cref="MessageType"/> of the message</param>
	/// <returns>a <see cref="ServiceResult{T}"/> that indicates a service callw as successful, along with the result payload</returns>
	public static ServiceResult<T> Ok(
		T result,
		string? message = null,
		MessageType messageType = MessageType.Success) => new()
	{
		ServiceError = ServiceError.None,
		Result = result,
		Message = message,
		MessageType = messageType
	};

	/// <summary>
	/// Creates a failed <see cref="ServiceResult{T}"/> with the specified error information
	/// </summary>
	/// <param name="errorType">The <see cref="ServiceError"/> representing the result of the service</param>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <param name="messageType">The <see cref="MessageType"/> of the error message</param>
	/// <returns></returns>
	public static ServiceResult<T> Failure(
		ServiceError errorType,
		string? errorMessage,
		MessageType? messageType) => new()
	{
		ServiceError = errorType,
		Message = errorMessage,
		MessageType = messageType ?? MessageType.Error
	};

	/// <summary>
	/// Creates a failed <see cref="ServiceResult{T}"/> indicating the specified resource couldn't be found
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <param name="messageType">The <see cref="MessageType"/> of the error message</param>
	/// <returns></returns>
	public static ServiceResult<T> NotFound(
		string? errorMessage = null,
		MessageType? messageType = null) => Failure(ServiceError.NotFound, errorMessage, messageType);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult{T}"/> indicating the current user is not authenticated
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <param name="messageType">The <see cref="MessageType"/> of the error message</param>
	/// <returns></returns>
	public static ServiceResult<T> Unauthorized(
		string? errorMessage = null,
		MessageType? messageType = null) => Failure(ServiceError.Unauthorized, errorMessage, messageType);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult{T}"/> indicating the current user is not authorized to perform an action
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <param name="messageType">The <see cref="MessageType"/> of the error message</param>
	/// <returns></returns>
	public static ServiceResult<T> Forbidden(
		string? errorMessage = null,
		MessageType? messageType = null) => Failure(ServiceError.Forbidden, errorMessage, messageType);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult{T}"/> indicating the request could not be processed with the given data
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <param name="messageType">The <see cref="MessageType"/> of the error message</param>
	/// <returns></returns>
	public static ServiceResult<T> Unprocessable(
		string? errorMessage = null,
		MessageType? messageType = null) => Failure(ServiceError.Unprocessable, errorMessage, messageType);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult{T}"/> indicating there was conflicting data that prevented the request from processing
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <param name="messageType">The <see cref="MessageType"/> of the error message</param>
	/// <returns></returns>
	public static ServiceResult<T> Conflict(
		string? errorMessage = null,
		MessageType? messageType = null) => Failure(ServiceError.DataConflict, errorMessage, messageType);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult{T}"/> indicating the resource being updated was modified by another user
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <param name="messageType">The <see cref="MessageType"/> of the error message</param>
	/// <returns></returns>
	public static ServiceResult<T> ConcurrencyConflict(
		string? errorMessage = null,
		MessageType? messageType = null) => Failure(ServiceError.DatabaseConcurrency, errorMessage, messageType);

	public bool WasSuccessful => ServiceError == ServiceError.None;

	public ServiceError ServiceError { get; set; }

	public MessageType MessageType { get; set; }

	public T? Result { get; set; }

	public string? Message { get; set; }
}