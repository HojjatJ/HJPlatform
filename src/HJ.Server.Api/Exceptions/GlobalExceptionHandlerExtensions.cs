using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HJ.Server.Application.Products;
using HJ.Server.Application.Installations;

namespace HJ.Server.Api.Exceptions;

public static class GlobalExceptionHandlerExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                var (statusCode, title) = exception switch
                {
                    ProductAlreadyExistsException => (StatusCodes.Status409Conflict, "Conflict"),
                    _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = exception?.Message,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            });
        });
    }
}
