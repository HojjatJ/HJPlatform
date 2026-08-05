$root = "D:\Projects\Visual Studio\HJPlatform"

$foundation = "$root\src\HJ.Server.Foundation"


$folders = @(
    "$foundation\Abstractions",
    "$foundation\Constants",
    "$foundation\Exceptions",
    "$foundation\Extensions"
)


foreach ($folder in $folders)
{
    if (!(Test-Path $folder))
    {
        New-Item -ItemType Directory -Path $folder | Out-Null
        Write-Host "Created: $folder"
    }
}


# IDateTimeProvider

@"
namespace HJ.Server.Foundation.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
"@ | Set-Content "$foundation\Abstractions\IDateTimeProvider.cs" -Encoding UTF8



# SystemConstants

@"
namespace HJ.Server.Foundation.Constants;

public static class SystemConstants
{
    public const string ApplicationName = "HJPlatform";

    public const string DefaultCulture = "fa-IR";
}
"@ | Set-Content "$foundation\Constants\SystemConstants.cs" -Encoding UTF8



# HJException

@"
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
"@ | Set-Content "$foundation\Exceptions\HJException.cs" -Encoding UTF8



# ResultExtensions

@"
using Ardalis.Result;

namespace HJ.Server.Foundation.Extensions;

public static class ResultExtensions
{
    public static Result<T> Success<T>(this T value)
    {
        return Result<T>.Success(value);
    }
}
"@ | Set-Content "$foundation\Extensions\ResultExtensions.cs" -Encoding UTF8



Write-Host ""
Write-Host "Foundation base created successfully."