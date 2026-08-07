using System;

namespace HJ.Server.Domain.Installations.Exceptions;

public class InstallationAlreadyExistsException : Exception
{
    public Guid InstallationId { get; }

    public InstallationAlreadyExistsException(Guid installationId) 
        : base($"Installation with ID '{installationId}' already exists.")
    {
        InstallationId = installationId;
    }
}