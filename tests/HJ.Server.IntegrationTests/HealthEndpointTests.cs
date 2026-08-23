using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HJ.Server.IntegrationTests;

public class HealthEndpointTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.IntegrationTests.json", optional: false);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Health_Should_Return_Ok()
    {
        var response = await _client.GetAsync("/api/health");

        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        content.Should()
            .Contain("HJPlatform");
    }
}
