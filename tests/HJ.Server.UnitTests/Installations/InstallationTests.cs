using System;
using Xunit;
using Installation = HJ.Server.Domain.Installations.Installation;
using InstallationEnvironment = HJ.Server.Domain.Installations.InstallationEnvironment;

namespace HJ.Server.UnitTests.Installations;

public class InstallationTests
{
    [Fact]
    public void Create_WithValidInputs_ShouldCreateInstallation()
    {
        // Arrange
        var installationId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var versionId = Guid.NewGuid();
        var tenantId = Guid.NewGuid();

        // Act
        var installation = Installation.Create(installationId, productId, versionId, tenantId);

        // Assert
        Assert.NotNull(installation);
        Assert.Equal(installationId, installation.InstallationId);
        Assert.Equal(productId, installation.ProductId);
        Assert.Equal(versionId, installation.ProductVersionId);
        Assert.Equal(tenantId, installation.TenantId);
        Assert.Equal(installation.FirstSeenAt, installation.LastSeenAt);
        Assert.True(installation.FirstSeenAt <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", "d3b07384-d113-4359-bc3d-82d8c30d9703", "d3b07384-d113-4359-bc3d-82d8c30d9703")]
    [InlineData("d3b07384-d113-4359-bc3d-82d8c30d9703", "00000000-0000-0000-0000-000000000000", "d3b07384-d113-4359-bc3d-82d8c30d9703")]
    [InlineData("d3b07384-d113-4359-bc3d-82d8c30d9703", "d3b07384-d113-4359-bc3d-82d8c30d9703", "00000000-0000-0000-0000-000000000000")]
    public void Create_WithEmptyGuids_ShouldThrowArgumentException(string instId, string prodId, string verId)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Installation.Create(Guid.Parse(instId), Guid.Parse(prodId), Guid.Parse(verId), null));
    }

    [Fact]
    public void RecordHeartbeat_ShouldUpdateLastSeenAtAndModifiedAt()
    {
        // Arrange
        var installation = Installation.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), null);
        var initialLastSeenAt = installation.LastSeenAt;
        
        System.Threading.Thread.Sleep(5);

        // Act
        installation.RecordHeartbeat();

        // Assert
        Assert.True(installation.LastSeenAt > initialLastSeenAt);
        Assert.NotNull(installation.ModifiedAt);
    }

    [Fact]
    public void UpdateVersion_ShouldUpdateVersionAndLastSeenAt()
    {
        // Arrange
        var installation = Installation.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), null);
        var newVersionId = Guid.NewGuid();
        var initialLastSeenAt = installation.LastSeenAt;

        System.Threading.Thread.Sleep(5);

        // Act
        installation.UpdateVersion(newVersionId);

        // Assert
        Assert.Equal(newVersionId, installation.ProductVersionId);
        Assert.True(installation.LastSeenAt > initialLastSeenAt);
    }

    [Fact]
    public void SetEnvironment_ShouldSetEnvironmentPropertyAndModifiedAt()
    {
        // Arrange
        var installation = Installation.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), null);
        var environment = InstallationEnvironment.Create(installation.Id, "Win11", "Intel i7", 8, 16.0, "1920x1080", "HW123");
        
        System.Threading.Thread.Sleep(5);

        // Act
        installation.SetEnvironment(environment);

        // Assert
        Assert.NotNull(installation.Environment);
        Assert.Equal("Win11", installation.Environment.OSVersion);
        Assert.Equal("HW123", installation.Environment.HardwareIdentifier);
        Assert.NotNull(installation.ModifiedAt);
    }
}