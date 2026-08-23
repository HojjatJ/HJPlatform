using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HJ.Server.Foundation.Abstractions.Logging;
using HJ.Server.Domain.Logging;
using HJ.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HJ.Server.Infrastructure.Logging;

public class LoggingService : ILoggingService
{
    private readonly HJDbContext _dbContext;
    private readonly ILogger<LoggingService> _logger;

    public LoggingService(HJDbContext dbContext, ILogger<LoggingService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task LogAsync(
        string level,
        string message,
        Guid? installationId,
        Guid? operationId,
        object? exception,
        object? properties,
        CancellationToken cancellationToken)
    {
        try
        {
            var appLog = ApplicationLog.Create(
                level,
                message,
                installationId ?? Guid.Empty,
                operationId ?? Guid.Empty,
                exception != null ? JsonSerializer.Serialize(new { Message = exception.ToString() }) : null,
                properties != null ? JsonSerializer.Serialize(properties) : null
            );

            _dbContext.ApplicationLogs.Add(appLog);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist application log: {Message}", message);
        }
    }
}
