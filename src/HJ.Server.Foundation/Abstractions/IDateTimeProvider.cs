namespace HJ.Server.Foundation.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
