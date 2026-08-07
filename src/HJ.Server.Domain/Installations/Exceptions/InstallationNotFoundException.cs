using System;

namespace HJ.Server.Domain.Installations.Exceptions;

public class InstallationNotFoundException : Exception
{
    public Guid InstallationId { get; }

    public InstallationNotFoundException(Guid installationId) 
        : base($"Installation with ID '{installationId}' was not found.")
    {
        InstallationId = installationId;
    }
}