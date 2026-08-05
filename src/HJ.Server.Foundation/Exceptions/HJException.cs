namespace HJ.Server.Foundation.Exceptions;

public class HJException : Exception
{
    public string Code { get; }

    public HJException(
        string code,
        string message)
        : base(message)
    {
        Code = code;
    }
}
