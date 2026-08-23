using System;

namespace HJ.Server.Contracts.Operations.Requests;

public record StartOperationRequest(
    Guid InstallationId,
    string Type);
