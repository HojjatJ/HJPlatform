using System;

namespace HJ.Server.Domain.Installations.Exceptions;

public class InstallationAlreadyExistsException : Exception
{
    public InstallationAlreadyExistsException(Guid installationId)
        : base($"An installation with InstallationId '{installationId}' already exists.")
    {
    }
}