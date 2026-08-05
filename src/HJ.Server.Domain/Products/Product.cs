using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Products;

public class Product : BaseEntity
{
    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; } = true;

    private Product()
    {
    }

    public Product(string code, string name, string? description = null)
    {
        Code = code;
        Name = name;
        Description = description;
    }
}
