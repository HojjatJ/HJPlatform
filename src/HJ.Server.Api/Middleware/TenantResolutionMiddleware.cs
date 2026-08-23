using HJ.Server.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Http;

namespace HJ.Server.Api.Middleware;

public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(
        HttpContext context,
        TenantContext tenantContext)
    {
        var claim = context.User.FindFirst("tenant_id");

        if (claim is not null &&
            Guid.TryParse(claim.Value, out var tenantId))
        {
            tenantContext.SetTenant(tenantId);
        }
        else if (RequiresTenant(context))
        {
            throw new UnauthorizedAccessException(
                "Tenant context is required but missing.");
        }

        await _next(context);
    }

    private static bool RequiresTenant(HttpContext context) =>
        context.User.Identity?.IsAuthenticated == true
        && !context.Request.Path.StartsWithSegments("/admin");
}
