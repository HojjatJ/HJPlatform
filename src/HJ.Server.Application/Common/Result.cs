using System;

namespace HJ.Server.Application.Common;

public sealed record Error(
    string Code,
    string Description,
    ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None =
        new(string.Empty, string.Empty, ErrorType.Failure);

    public static readonly Error NullValue =
        new(
            "Error.NullValue",
            "A null value was provided.",
            ErrorType.Failure);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException(
                "A successful result cannot contain an error.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException(
                "A failed result must contain an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
}

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "Cannot access value of a failed result.");

    private Result(
        TValue? value,
        bool isSuccess,
        Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(
            value,
            true,
            Error.None);
    }

    public static new Result<TValue> Failure(Error error)
    {
        return new Result<TValue>(
            default,
            false,
            error);
    }
}
