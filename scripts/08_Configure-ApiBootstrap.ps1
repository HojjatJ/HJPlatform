$root = "D:\Projects\Visual Studio\HJPlatform"

$file = "$root\src\HJ.Server.Api\Program.cs"


$content = @"
using FastEndpoints;
using FastEndpoints.Swagger;
using Hellang.Middleware.ProblemDetails;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


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


// -------------------------
// ProblemDetails
// -------------------------

builder.Services.AddProblemDetails();



var app = builder.Build();


// -------------------------
// Middleware
// -------------------------

app.UseSerilogRequestLogging();

app.UseProblemDetails();


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
"@


Set-Content `
    -Path $file `
    -Value $content `
    -Encoding UTF8


Write-Host "API bootstrap configured."