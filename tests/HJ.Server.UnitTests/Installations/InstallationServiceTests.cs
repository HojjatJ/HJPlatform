using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using FluentAssertions;
using HJ.Server.Application.Installations;
using HJ.Server.Domain.Installations;
using HJ.Server.Contracts.Installations;

namespace HJ.Server.UnitTests.Installations;

public class InstallationServiceTests
{
    [Fact]
    public async Task RecordHeartbeatAsync_Should_Not_Mutate_Version_Or_Environment()
    {
        var repository = Substitute.For<IInstallationRepository>();
        var mapper = new InstallationMapper();
        var service = new InstallationService(repository, mapper);
        
        var installationId = Guid.NewGuid();
        var initialVersionId = Guid.NewGuid();
        var initialEnv = InstallationEnvironment.Create(installationId, "Win10", "CPU", 4, 16, "1920x1080", "HWID");
        
        var installation = Installation.Create(installationId, Guid.NewGuid(), initialVersionId, null);
        installation.SetEnvironment(initialEnv);
        
        repository.GetByInstallationIdAsync(installationId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Installation?>(installation));

        var request = new RecordHeartbeatRequest
        {
            ProductVersionId = Guid.NewGuid(), // Different version
            HardwareIdentifier = "NEW-HWID",
            OSVersion = "Win11" // Different env
        };

        await service.RecordHeartbeatAsync(installationId, request);

        installation.ProductVersionId.Should().Be(initialVersionId);
        installation.Environment.Should().BeSameAs(initialEnv);
    }
}