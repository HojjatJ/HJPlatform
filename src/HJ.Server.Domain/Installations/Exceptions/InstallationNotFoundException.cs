using System;

namespace HJ.Server.Domain.Installations.Exceptions;

public class InstallationNotFoundException : Exception
{
    public InstallationNotFoundException(Guid identifier)
        : base($"Installation with identifier '{identifier}' was not found.")
    {
    }
}