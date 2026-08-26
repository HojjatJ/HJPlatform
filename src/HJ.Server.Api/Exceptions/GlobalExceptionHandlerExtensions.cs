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
                    ProductAlreadyExistsException => (StatusCodes.Status409Conflict, "Conflict"),
                    OperationAlreadyCompletedException => (StatusCodes.Status400BadRequest, "Bad Request"),
                    OperationNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
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
