$file = "tests/HJ.Server.ArchitectureTests/LayerDependencyTests.cs"

$content = @'
using NetArchTest.Rules;
using Xunit;

namespace HJ.Server.ArchitectureTests;

public class LayerDependencyTests
{
    [Fact]
    public void Domain_Should_Not_Depend_On_Application_Or_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(HJ.Server.Domain.Class1).Assembly)
            .ShouldNot()
            .HaveDependencyOn("HJ.Server.Application")
            .And()
            .ShouldNot()
            .HaveDependencyOn("HJ.Server.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, result.Message);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(HJ.Server.Application.Class1).Assembly)
            .ShouldNot()
            .HaveDependencyOn("HJ.Server.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, result.Message);
    }
}
'@

Set-Content -Path $file -Value $content -Encoding UTF8

Write-Host "Architecture rules created."