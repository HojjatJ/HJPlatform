using HJ.Server.Contracts.Products;
using HJ.Server.Contracts.Products.Requests;

namespace HJ.Server.Application.Products;

public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductRequest request);
}
