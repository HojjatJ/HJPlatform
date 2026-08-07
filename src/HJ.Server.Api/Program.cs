using HJ.Server.Application.DependencyInjection;
using HJ.Server.Infrastructure.DependencyInjection;
using HJ.Server.Api.Exceptions;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Application and Infrastructure DI (API does not configure EF Core directly)
builder.Services.AddHJApplication();
builder.Services.AddHJInfrastructure();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "HJ Platform API";
        s.Version = "v1";
    };
});

var app = builder.Build();

// Centralized Exception Handling Middleware
app.ConfigureExceptionHandler();

// Standard Middleware Pipeline
app.UseHttpsRedirection();

// Authentication & Authorization Pipeline Placeholders
// app.UseAuthentication();
// app.UseAuthorization();

app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();

public partial class Program { }
