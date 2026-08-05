namespace HJ.Server.Contracts.Products.Requests;

public class CreateProductRequest
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
