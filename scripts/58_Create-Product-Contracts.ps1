$root = Split-Path -Parent $PSScriptRoot

$path = "$root\src\HJ.Server.Contracts\Products"

New-Item -ItemType Directory -Path $path -Force | Out-Null
New-Item -ItemType Directory -Path "$path\Requests" -Force | Out-Null

@"
namespace HJ.Server.Contracts.Products;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}
"@ | Set-Content "$path\ProductDto.cs"


@"
namespace HJ.Server.Contracts.Products;

public class ProductVersionDto
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string Version { get; set; } = string.Empty;

    public string? BuildNumber { get; set; }

    public string? ReleaseNotes { get; set; }

    public DateTime ReleaseDate { get; set; }

    public int Status { get; set; }

    public int UpdatePolicy { get; set; }
}
"@ | Set-Content "$path\ProductVersionDto.cs"


@"
namespace HJ.Server.Contracts.Products.Requests;

public class CreateProductRequest
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
"@ | Set-Content "$path\Requests\CreateProductRequest.cs"


Write-Host "Product contracts created."