using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HJ.Server.Contracts.Operations;
using HJ.Server.Contracts.Operations.Requests;
using HJ.Server.Foundation.Abstractions.Telemetry;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HJ.Server.IntegrationTests.Telemetry;

public class TelemetryIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TelemetryIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("IntegrationTests");
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddJsonFile("appsettings.IntegrationTests.json", optional: false);
            });
        });
    }

    [Fact]
    public async Task StartOperation_WithNormalTelemetry_ShouldSucceed()
    {
        // Scenario 1: Successful TrackEvent / telemetry operation (via start operation).
        var client = _factory.CreateClient();
        var request = new StartOperationRequest(Guid.NewGuid(), "NormalTelemetryOperation", null);
        var response = await client.PostAsJsonAsync("/api/operations", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<OperationDto>();
        result.Should().NotBeNull();
        result!.Status.Should().Be(OperationStatusDto.Started);
    }

    [Fact]
    public async Task StartOperation_WithFailingTelemetry_ShouldStillSucceed()
    {
        // Scenario 2: Telemetry failure is isolated.
        // We override ITelemetryService with a poison service that always throws an exception.
        var failingFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Scoped<ITelemetryService, PoisonTelemetryService>());
            });
        });

        var client = failingFactory.CreateClient();
        var request = new StartOperationRequest(Guid.NewGuid(), "PoisonTelemetryOperation", null);
        
        // This business operation calls TelemetryService.TrackEventAsync internally.
        // It must succeed despite the telemetry service throwing.
        var response = await client.PostAsJsonAsync("/api/operations", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<OperationDto>();
        result.Should().NotBeNull();
        result!.Status.Should().Be(OperationStatusDto.Started);
    }

    private class PoisonTelemetryService : ITelemetryService
    {
        public Task TrackEventAsync(
            string eventName,
            int version,
            object payload,
            Guid? installationId,
            Guid? operationId,
            CancellationToken cancellationToken)
        {
            throw new Exception("Poison telemetry failure! This exception should be caught and logged, not crash the process.");
        }
    }
}
