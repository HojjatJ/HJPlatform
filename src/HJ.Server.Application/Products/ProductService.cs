using HJ.Server.Contracts.Products;
using HJ.Server.Contracts.Products.Requests;
using HJ.Server.Domain.Products;

namespace HJ.Server.Application.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto> CreateAsync(
        CreateProductRequest request)
    {
        var exists = await _repository.ExistsAsync(
            request.Code);

        if (exists)
        {
            throw new InvalidOperationException(
                "Product code already exists.");
        }

        var product = new Product(
            request.Code,
            request.Name,
            request.Description);

        await _repository.AddAsync(product);

        return new ProductDto
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name,
            Description = product.Description,
            IsActive = product.IsActive
        };
    }
}

