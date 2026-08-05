$root = Split-Path -Parent $PSScriptRoot

$path = "$root\src\HJ.Server.Api\Endpoints\Products"

New-Item -ItemType Directory -Path $path -Force | Out-Null

@"
using FastEndpoints;
using HJ.Server.Application.Products;
using HJ.Server.Contracts.Products.Requests;

namespace HJ.Server.Api.Endpoints.Products;

public class CreateProductEndpoint 
    : Endpoint<CreateProductRequest, ProductDto>
{
    private readonly IProductService _service;

    public CreateProductEndpoint(
        IProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("/api/products");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Creates a new product.";
            s.Description = "Creates a product in the system.";
        });
    }

    public override async Task HandleAsync(
        CreateProductRequest req,
        CancellationToken ct)
    {
        var result = await _service.CreateAsync(req);

        await Send.OkAsync(result, ct);
    }
}
"@ | Set-Content "$path\CreateProductEndpoint.cs"

Write-Host "Create product endpoint created."