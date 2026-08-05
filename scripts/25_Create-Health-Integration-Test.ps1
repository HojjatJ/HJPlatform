$root = "D:\Projects\Visual Studio\HJPlatform"

$testFolder = "$root\tests\HJ.Server.IntegrationTests"


@"
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HJ.Server.IntegrationTests;


public class HealthEndpointTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;


    public HealthEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task Health_Should_Return_Ok()
    {
        var response = await _client.GetAsync("/health");


        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);


        var content = await response.Content.ReadAsStringAsync();


        content.Should()
            .Contain("HJPlatform");
    }
}
"@ | Set-Content `
"$testFolder\HealthEndpointTests.cs" `
-Encoding UTF8


Write-Host "Health integration test created."