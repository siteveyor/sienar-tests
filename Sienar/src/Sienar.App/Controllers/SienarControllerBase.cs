using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sienar.Errors;
using Sienar.Infrastructure;

namespace Sienar.Controllers;

public abstract class SienarControllerBase<TController, TService> : ControllerBase
{
	protected readonly ILogger<TController> Logger;
	protected readonly TService Service;

	/// <inheritdoc />
	public SienarControllerBase(
		ILogger<TController> logger,
		TService service)
	{
		Logger = logger;
		Service = service;
	}

	protected async Task<IActionResult> ProcessServiceCall<T>(
		Func<Task<ServiceResult<T>>> action,
	    HttpStatusCode successStatusCode = HttpStatusCode.OK)
	{
		try
		{
			var result = await action();
			if (!result.WasSuccessful)
			{
				return GenerateErrorResponse(result);
			}

			return result.Result is null || result.Result.Equals(default(T))
				? StatusCode((int)HttpStatusCode.NoContent)
				: StatusCode((int)successStatusCode, result.Result);
		}
		catch (SienarSpambotException e)
		{
			Logger.LogWarning(e, "Spambot detected");
			return NoContent();
		}
		catch (Exception e)
		{
			Logger.LogError(e, ErrorMessages.Generic.UnhandledByServiceLayer);
			return GenerateErrorResponse(e);
		}
	}

	protected async Task<IActionResult> ProcessServiceCallReturningFile(
		Func<Task<ServiceResult<FileDto>>> action)
	{
		try
		{
			var result = await action();
			if (!result.WasSuccessful)
			{
				return GenerateErrorResponse(result);
			}

			var file = result.Result!;
			Response.Headers.Add("Content-Disposition", $"attachment; filename=${file.Name}");
			return new FileContentResult(file.Contents, file.Mime);
		}
		catch (SienarSpambotException e)
		{
			Logger.LogWarning(e, "Spambot detected");
			return NoContent();
		}
		catch (Exception e)
		{
			Logger.LogError(e, ErrorMessages.Generic.UnhandledByServiceLayer);
			return GenerateErrorResponse(e);
		}
	}

	protected IActionResult GenerateErrorResponse<T>(ServiceResult<T> result)
	{
		HttpStatusCode status;
		var errorMessage = result.Message;

		switch (result.ServiceError)
		{
			case ServiceError.None:
				throw new InvalidOperationException($"Unable to generate an error response without a valid service error. Error provided: {nameof(ServiceError.None)}");
			case ServiceError.NotFound:
				status = HttpStatusCode.NotFound;
				errorMessage ??= ErrorMessages.Generic.NotFound;
				break;
			case ServiceError.Unauthorized:
				status = HttpStatusCode.Unauthorized;
				errorMessage ??= ErrorMessages.Generic.NotLoggedIn;
				break;
			case ServiceError.Forbidden:
				status = HttpStatusCode.Forbidden;
				errorMessage ??= ErrorMessages.Generic.NoPermission;
				break;
			case ServiceError.Unprocessable:
				status = HttpStatusCode.UnprocessableEntity;
				errorMessage ??= ErrorMessages.Generic.Unprocessable;
				break;
			case ServiceError.DataConflict:
				status = HttpStatusCode.Conflict;
				errorMessage ??= ErrorMessages.Generic.DataConflict;
				break;
			case ServiceError.DatabaseConcurrency:
				status = HttpStatusCode.Conflict;
				errorMessage ??= ErrorMessages.Generic.DataConcurrencyConflict;
				break;
			case ServiceError.Unknown:
			default:
				status = HttpStatusCode.InternalServerError;
				errorMessage ??= ErrorMessages.Generic.Unknown;
				break;
		}

		return GenerateErrorResponse(errorMessage, status);
	}

	protected IActionResult GenerateErrorResponse(Exception e)
	{
		var message = e is SienarException
			? e.Message
			: ErrorMessages.Generic.Unknown;

		var status = e switch
		{
			SienarConflictException => HttpStatusCode.Conflict,
			SienarNotFoundException => HttpStatusCode.NotFound,
			SienarUnauthorizedException => HttpStatusCode.Forbidden,
			SienarUnprocessableEntityException => HttpStatusCode.UnprocessableEntity,
			_ => HttpStatusCode.InternalServerError
		};

		return GenerateErrorResponse(message, status);
	}

	protected IActionResult GenerateErrorResponse(
		string errorMessage,
		HttpStatusCode statusCode)
	{
		var dto = new ErrorDto($"{errorMessage} If you continue to have problems, please contact the IT team.");

		return StatusCode((int)statusCode, dto);
	}
}