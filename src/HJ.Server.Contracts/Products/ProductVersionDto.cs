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
