namespace Sienar;

public enum ServiceError
{
	None,
	NotFound,
	Unauthorized,
	Forbidden,
	Unprocessable,
	DataConflict,
	DatabaseConcurrency,
	Unknown
}