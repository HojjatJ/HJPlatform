using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HJ.Server.Application.Products;
using HJ.Server.Application.Installations;
using HJ.Server.Domain.Operations.Exceptions;

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
                    null => (StatusCodes.Status500InternalServerError, "Internal Server Error"),
                    _ => exception.GetType().Name switch
                    {
                        var name when name.EndsWith("NotFoundException") => (StatusCodes.Status404NotFound, "Not Found"),
                        var name when name.EndsWith("AlreadyExistsException") => (StatusCodes.Status409Conflict, "Conflict"),
                        _ when exception.GetType().Namespace?.StartsWith("HJ.Server.Domain") == true => (StatusCodes.Status400BadRequest, "Bad Request"),
                        _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
                    }
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
