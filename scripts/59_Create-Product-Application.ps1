$root = Split-Path -Parent $PSScriptRoot

$path = "$root\src\HJ.Server.Application\Products"

New-Item -ItemType Directory -Path "$path\Validators" -Force | Out-Null

@"
using HJ.Server.Contracts.Products;
using HJ.Server.Contracts.Products.Requests;

namespace HJ.Server.Application.Products;

public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductRequest request);
}
"@ | Set-Content "$path\IProductService.cs"


@"
using HJ.Server.Contracts.Products;
using HJ.Server.Contracts.Products.Requests;

namespace HJ.Server.Application.Products;

public class ProductService : IProductService
{
    public Task<ProductDto> CreateAsync(CreateProductRequest request)
    {
        var product = new ProductDto
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsActive = true
        };

        return Task.FromResult(product);
    }
}
"@ | Set-Content "$path\ProductService.cs"


@"
using FluentValidation;
using HJ.Server.Contracts.Products.Requests;

namespace HJ.Server.Application.Products.Validators;

public class CreateProductRequestValidator 
    : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
"@ | Set-Content "$path\Validators\CreateProductRequestValidator.cs"


Write-Host "Product application layer created."