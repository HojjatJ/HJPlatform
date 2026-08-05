$root = "D:\Projects\Visual Studio\HJPlatform"

$project = "$root\tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"


dotnet test $project --list-tests --verbosity normal