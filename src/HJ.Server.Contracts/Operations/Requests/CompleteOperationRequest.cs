using HJ.Server.Contracts.Operations;

namespace HJ.Server.Contracts.Operations.Requests;

public record CompleteOperationRequest(
    OperationStatusDto Status);
