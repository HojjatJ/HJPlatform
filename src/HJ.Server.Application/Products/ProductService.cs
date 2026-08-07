using System;
using System.Threading;
using System.Threading.Tasks;
using HJ.Server.Contracts.Products;
using HJ.Server.Contracts.Products.Requests;
using HJ.Server.Domain.Products;

namespace HJ.Server.Application.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        Guid? tenantId = null;

        var existing = await _productRepository.GetByCodeAsync(request.Code, tenantId, cancellationToken);
        if (existing is not null)
        {
            throw new ProductAlreadyExistsException(request.Code);
        }

        // Using standard object creation or factory method safely
        var product = new Product(request.Code, request.Name);
        
        await _productRepository.AddProductAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name
        };
    }
}
