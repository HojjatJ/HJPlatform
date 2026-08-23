using System;
using System.Threading;
using System.Threading.Tasks;
using HJ.Server.Contracts.Installations;

namespace HJ.Server.Application.Installations;

public interface IInstallationService
{
    Task<InstallationDto> RegisterAsync(RegisterInstallationRequest request, CancellationToken cancellationToken = default);
    Task<InstallationDto> GetAsync(Guid installationId, CancellationToken cancellationToken = default);
    Task RecordHeartbeatAsync(Guid installationId, RecordHeartbeatRequest request, CancellationToken cancellationToken = default);
    Task<InstallationDto> SetEnvironmentAsync(Guid installationId, SetInstallationEnvironmentRequest request, CancellationToken cancellationToken = default);
    Task<InstallationDto> UpdateVersionAsync(Guid installationId, UpdateInstallationVersionRequest request, CancellationToken cancellationToken = default);
}
