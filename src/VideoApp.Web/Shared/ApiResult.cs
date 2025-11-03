namespace VideoApp.Web.Shared;

public sealed record ApiResult(bool IsSuccess, Error Error)
{
    public static ApiResult Success() => new(true, Error.None);
    public static ApiResult Failure(Error error) => new(false, error);
}

public sealed record ApiResult<T>(bool IsSuccess, T? Value, Error Error)
{
    public static ApiResult<T> Success(T value) => new(true, value, Error.None);
    public static ApiResult<T> Failure(Error error) => new(false, default, error);

    public static implicit operator ApiResult<T>(T value) => Success(value);
    public static implicit operator ApiResult<T>(Error error) => Failure(error);
}

public sealed record Error(string Code, string Message, int? Status = null, object? Details = null)
{
    public static readonly Error None = new("", "");
}

public static class Errors
{
    public static Error BadRequest(string code, string message, object? details = null)
        => new(code, message, StatusCodes.Status400BadRequest, details);

    public static Error NotFound(string code, string message, object? details = null)
        => new(code, message, StatusCodes.Status404NotFound, details);

    public static Error Unauthorized(string code, string message, object? details = null)
        => new(code, message, StatusCodes.Status401Unauthorized, details);

    public static Error Forbidden(string code, string message, object? details = null)
        => new(code, message, StatusCodes.Status403Forbidden, details);

    public static Error Conflict(string code, string message, object? details = null)
        => new(code, message, StatusCodes.Status409Conflict, details);

    public static Error PayloadTooLarge(string code, string message, object? details = null)
        => new(code, message, StatusCodes.Status413PayloadTooLarge, details);

    public static Error Internal(string code, string message, object? details = null)
        => new(code, message, StatusCodes.Status500InternalServerError, details);
}