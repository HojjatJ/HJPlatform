using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using HJ.Server.Contracts.Operations;
using HJ.Server.Contracts.Operations.Requests;
using HJ.Server.Domain.Operations;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HJ.Server.IntegrationTests.Operations;

public class OperationIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OperationIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("IntegrationTests");
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddJsonFile("appsettings.IntegrationTests.json", optional: false);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Start_Should_Return_Created()
    {
        var request = new StartOperationRequest(Guid.NewGuid(), "TestOperation", null);
        var response = await _client.PostAsJsonAsync("/api/operations", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<OperationDto>();
        result.Should().NotBeNull();
        result!.Status.Should().Be(OperationStatusDto.Started);
    }

    [Fact]
    public async Task Complete_Should_Return_Ok()
    {
        var startRequest = new StartOperationRequest(Guid.NewGuid(), "CompleteTest", null);
        var startResponse = await _client.PostAsJsonAsync("/api/operations", startRequest);
        var operation = await startResponse.Content.ReadFromJsonAsync<OperationDto>();

        var completeRequest = new CompleteOperationRequest(OperationStatusDto.Completed);
        var response = await _client.PutAsJsonAsync($"/api/operations/{operation!.Id}/complete", completeRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<OperationDto>();
        result!.Status.Should().Be(OperationStatusDto.Completed);
    }

    [Fact]
    public async Task DoubleComplete_Should_Return_BadRequest()
    {
        var startRequest = new StartOperationRequest(Guid.NewGuid(), "DoubleCompleteTest", null);
        var startResponse = await _client.PostAsJsonAsync("/api/operations", startRequest);
        var operation = await startResponse.Content.ReadFromJsonAsync<OperationDto>();

        var completeRequest = new CompleteOperationRequest(OperationStatusDto.Completed);
        await _client.PutAsJsonAsync($"/api/operations/{operation!.Id}/complete", completeRequest);
        
        var secondResponse = await _client.PutAsJsonAsync($"/api/operations/{operation!.Id}/complete", completeRequest);

        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_Existing_Should_Return_Ok()
    {
        var startRequest = new StartOperationRequest(Guid.NewGuid(), "GetTest", null);
        var startResponse = await _client.PostAsJsonAsync("/api/operations", startRequest);
        var operation = await startResponse.Content.ReadFromJsonAsync<OperationDto>();

        var response = await _client.GetAsync($"/api/operations/{operation!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<OperationDto>();
        result!.Id.Should().Be(operation.Id);
    }

    [Fact]
    public async Task Get_NotFound_Should_Return_NotFound()
    {
        var response = await _client.GetAsync($"/api/operations/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
