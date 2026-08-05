using HJ.Server.Application.DependencyInjection;
using HJ.Server.Infrastructure.DependencyInjection;
using FastEndpoints;
using FastEndpoints.Swagger;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHJApplication();


// -------------------------
// Serilog
// -------------------------

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console();
});


// -------------------------
// FastEndpoints
// -------------------------

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();


// Infrastructure

builder.Services.AddHJInfrastructure(
    builder.Configuration);


// -------------------------
// ProblemDetails
// -------------------------

builder.Services.AddProblemDetails();



var app = builder.Build();


// -------------------------
// Middleware
// -------------------------

app.UseSerilogRequestLogging();



// -------------------------
// API
// -------------------------

app.UseFastEndpoints();


// -------------------------
// Scalar
// -------------------------

app.MapScalarApiReference(options =>
{
    options
        .WithTitle("HJPlatform API")
        .WithTheme(ScalarTheme.Mars);
});


app.Run();






