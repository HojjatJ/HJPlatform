using Riok.Mapperly.Abstractions;
using HJ.Server.Domain.Installations;
using HJ.Server.Contracts.Installations;

namespace HJ.Server.Application.Installations;

[Mapper]
public partial class InstallationMapper
{
    [MapperIgnoreSource("TenantId")]
    [MapperIgnoreSource("CreatedAt")]
    [MapperIgnoreSource("ModifiedAt")]
    public partial InstallationDto InstallationToDto(Installation installation);

    [MapperIgnoreSource("Id")]
    [MapperIgnoreSource("InstallationId")]
    [MapperIgnoreSource("CreatedAt")]
    public partial InstallationEnvironmentDto EnvironmentToDto(InstallationEnvironment environment);
}