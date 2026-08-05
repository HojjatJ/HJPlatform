$root = Split-Path -Parent $PSScriptRoot

$file = "$root\src\HJ.Server.Application\Products\ProductService.cs"

$content = Get-Content $file -Raw

$content = $content.Replace(
@"
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
"@,
@"
        var product = new Product(
            request.Code,
            request.Name,
            request.Description);
"@
)

Set-Content $file $content

Write-Host "ProductService entity creation fixed."