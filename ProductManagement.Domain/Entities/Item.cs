namespace ProductManagement.Domain.Entities;

public class Item
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Unit { get; set; }
    public Product? Product { get; set; }
}
