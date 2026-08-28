using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HJ.Server.Contracts.Operations;
using HJ.Server.Contracts.Operations.Requests;
using HJ.Server.Foundation.Abstractions.Logging;
using HJ.Server.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HJ.Server.IntegrationTests.Logging;

public class LoggingIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public LoggingIntegrationTests(WebApplicationFactory<Program> factory)
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
    public async Task LogAsync_WithValidParameters_ShouldPersistApplicationLogInDatabase()
    {
        // Scenario 1: Normal logging path works through the real DI container and database persistence.
        using var scope = _factory.Services.CreateScope();
        var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<HJDbContext>();

        var installationId = Guid.NewGuid();
        var operationId = Guid.NewGuid();
        var level = "Information";
        var message = "Integration test log message";
        var properties = new { Environment = "Test", Priority = 1 };

        await loggingService.LogAsync(
            level,
            message,
            installationId,
            operationId,
            null,
            properties,
            CancellationToken.None);

        var persistedLog = await dbContext.ApplicationLogs
            .FirstOrDefaultAsync(l => l.OperationId == operationId && l.InstallationId == installationId);

        persistedLog.Should().NotBeNull();
        persistedLog!.Level.Should().Be(level);
        persistedLog.Message.Should().Be(message);
        persistedLog.InstallationId.Should().Be(installationId);
        persistedLog.OperationId.Should().Be(operationId);
        persistedLog.PropertiesJson.Should().NotBeNull();
        persistedLog.PropertiesJson.Should().Contain("\"Environment\":\"Test\"");
        persistedLog.PropertiesJson.Should().Contain("\"Priority\":1");
    }

    [Fact]
    public async Task LogAsync_WithException_ShouldPersistApplicationLogWithExceptionDetails()
    {
        // Scenario 2: Logging with an exception object persists serialized exception details.
        using var scope = _factory.Services.CreateScope();
        var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<HJDbContext>();

        var installationId = Guid.NewGuid();
        var operationId = Guid.NewGuid();
        var level = "Error";
        var message = "Integration test error message";
        var exception = new InvalidOperationException("Simulated error for integration test");

        await loggingService.LogAsync(
            level,
            message,
            installationId,
            operationId,
            exception,
            null,
            CancellationToken.None);

        var persistedLog = await dbContext.ApplicationLogs
            .FirstOrDefaultAsync(l => l.OperationId == operationId && l.InstallationId == installationId);

        persistedLog.Should().NotBeNull();
        persistedLog!.Level.Should().Be(level);
        persistedLog.Message.Should().Be(message);
        persistedLog.ExceptionJson.Should().NotBeNull();
        persistedLog.ExceptionJson.Should().Contain("Simulated error for integration test");
    }

    [Fact]
    public async Task LogAsync_WhenInternalFailureOccurs_ShouldIsolateFailureAndNotThrow()
    {
        // Scenario 3: LoggingService internal failure isolation.
        // Even if invalid domain values are passed that cause internal creation errors,
        // LoggingService catches the exception and logs an error without throwing to the caller.
        using var scope = _factory.Services.CreateScope();
        var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

        var act = async () => await loggingService.LogAsync(
            string.Empty,
            string.Empty,
            Guid.Empty,
            Guid.Empty,
            null,
            null,
            CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task StartOperation_WithFailingLogging_ShouldStillSucceed()
    {
        // Scenario 4: Primary business operations succeed even when ILoggingService is replaced by a throwing/poison implementation.
        var failingFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Scoped<ILoggingService, PoisonLoggingService>());
            });
        });

        var client = failingFactory.CreateClient();
        var request = new StartOperationRequest(Guid.NewGuid(), "PoisonLoggingOperation", null);

        var response = await client.PostAsJsonAsync("/api/operations", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<OperationDto>();
        result.Should().NotBeNull();
        result!.Status.Should().Be(OperationStatusDto.Started);
    }

    private class PoisonLoggingService : ILoggingService
    {
        public Task LogAsync(
            string level,
            string message,
            Guid? installationId,
            Guid? operationId,
            object? exception,
            object? properties,
            CancellationToken cancellationToken)
        {
            throw new Exception("Poison logging failure! This exception should not crash the business operation.");
        }
    }
}
