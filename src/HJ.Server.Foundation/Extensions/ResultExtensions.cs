using Ardalis.Result;

namespace HJ.Server.Foundation.Extensions;

public static class ResultExtensions
{
    public static Result<T> Success<T>(this T value)
    {
        return Result<T>.Success(value);
    }
}
