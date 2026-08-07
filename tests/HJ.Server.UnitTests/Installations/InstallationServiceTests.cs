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
    public async Task GetAsync_Should_Return_InstallationDto()
    {
        var repository = Substitute.For<IInstallationRepository>();
        var mapper = new InstallationMapper();
        var service = new InstallationService(repository, mapper);
        
        var installationId = Guid.NewGuid();
        var installation = Installation.Create(installationId, Guid.NewGuid(), Guid.NewGuid(), null);
        
        repository.GetByInstallationIdAsync(installationId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Installation?>(installation));

        var result = await service.GetAsync(installationId);

        result.Should().NotBeNull();
        result.InstallationId.Should().Be(installationId);
    }
}