using System;
using System.Threading;
using System.Threading.Tasks;
using HJ.Server.Contracts.Installations;
using HJ.Server.Domain.Installations;
using HJ.Server.Domain.Installations.Exceptions;

namespace HJ.Server.Application.Installations;

public class InstallationService : IInstallationService
{
    private readonly IInstallationRepository _repository;
    private readonly InstallationMapper _mapper;

    public InstallationService(IInstallationRepository repository, InstallationMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<InstallationDto> RegisterAsync(RegisterInstallationRequest request, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(request.InstallationId, cancellationToken))
            throw new InstallationAlreadyExistsException(request.InstallationId);

        var installation = Installation.Create(request.InstallationId, request.ProductId, request.ProductVersionId, request.TenantId);

        await _repository.AddAsync(installation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.InstallationToDto(installation);
    }

    public async Task<InstallationDto> GetAsync(Guid installationId, CancellationToken cancellationToken = default)
    {
        var installation = await _repository.GetByInstallationIdAsync(installationId, cancellationToken)
            ?? throw new InstallationNotFoundException(installationId);

        return _mapper.InstallationToDto(installation);
    }

    public async Task RecordHeartbeatAsync(Guid installationId, RecordHeartbeatRequest request, CancellationToken cancellationToken = default)
    {
        var installation = await _repository.GetByInstallationIdAsync(installationId, cancellationToken)
            ?? throw new InstallationNotFoundException(installationId);

        if (request.ProductVersionId.HasValue && request.ProductVersionId.Value != installation.ProductVersionId)
        {
            installation.UpdateVersion(request.ProductVersionId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.HardwareIdentifier))
        {
            var env = InstallationEnvironment.Create(
                installation.Id,
                request.OSVersion,
                request.CpuName,
                request.CpuCoreCount ?? 0,
                request.RamGB ?? 0,
                request.ScreenResolution,
                request.HardwareIdentifier);
                
            installation.SetEnvironment(env);
        }

        installation.RecordHeartbeat();

        await _repository.UpdateAsync(installation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}