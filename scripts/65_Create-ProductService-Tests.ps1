$root = Split-Path -Parent $PSScriptRoot

$path = "$root\tests\HJ.Server.UnitTests\Products"

New-Item -ItemType Directory -Path $path -Force | Out-Null

@"
using FluentAssertions;
using HJ.Server.Application.Products;
using HJ.Server.Contracts.Products.Requests;
using HJ.Server.Domain.Products;
using Moq;
using Xunit;

namespace HJ.Server.UnitTests.Products;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldCreateProduct()
    {
        var repository = new Mock<IProductRepository>();

        repository
            .Setup(x => x.ExistsAsync(
                It.IsAny<string>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var service = new ProductService(repository.Object);

        var request = new CreateProductRequest
        {
            Code = "TEST",
            Name = "Test Product",
            Description = "Description"
        };

        var result = await service.CreateAsync(request);

        result.Should().NotBeNull();
        result.Code.Should().Be("TEST");
        result.Name.Should().Be("Test Product");

        repository.Verify(
            x => x.AddAsync(
                It.IsAny<Product>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task CreateAsync_WhenCodeExists_ShouldThrowException()
    {
        var repository = new Mock<IProductRepository>();

        repository
            .Setup(x => x.ExistsAsync(
                It.IsAny<string>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var service = new ProductService(repository.Object);

        var request = new CreateProductRequest
        {
            Code = "DUPLICATE",
            Name = "Duplicate Product"
        };

        Func<Task> action = async () =>
            await service.CreateAsync(request);

        await action.Should()
            .ThrowAsync<InvalidOperationException>();
    }
}
"@ | Set-Content "$path\ProductServiceTests.cs"

Write-Host "ProductService tests created."