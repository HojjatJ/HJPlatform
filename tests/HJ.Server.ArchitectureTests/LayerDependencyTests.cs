using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace HJ.Server.ArchitectureTests;

public class LayerDependencyTests
{
    private static Assembly DomainAssembly =>
        Assembly.Load("HJ.Server.Domain");

    private static Assembly ApplicationAssembly =>
        Assembly.Load("HJ.Server.Application");


    [Fact]
    public void Domain_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn("HJ.Server.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }


    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn("HJ.Server.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}