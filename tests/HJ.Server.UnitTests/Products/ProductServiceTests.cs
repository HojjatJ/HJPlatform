using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HJ.Server.Application.Products;
using HJ.Server.Contracts.Products.Requests;
using HJ.Server.Domain.Products;
using NSubstitute;
using Xunit;

public class ProductServiceTests
{
    private readonly IProductRepository _productRepositoryMock;
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _productRepositoryMock = Substitute.For<IProductRepository>();
        _sut = new ProductService(_productRepositoryMock);
    }

    [Fact]
    public async Task CreateAsync_WhenCodeExists_ShouldThrowException()
    {
        // Arrange
        var request = new CreateProductRequest { Code = "TEST", Name = "Test Product" };
        var existingProduct = new Product("TEST", "Existing");

        _productRepositoryMock
            .GetByCodeAsync("TEST", Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(existingProduct));

        // Act
        Func<Task> act = async () => await _sut.CreateAsync(request);

        // Assert
        await act.Should().ThrowAsync<ProductAlreadyExistsException>();
    }
}
