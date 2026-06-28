using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Mapping;

public static class ProductMapper
{
    public static ProductDto ToDto(Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Items.Select(ToItemDto).ToList());
    }

    public static ItemDto ToItemDto(Item item)
    {
        return new ItemDto(item.Id, item.ProductId, item.Sku, item.Quantity, item.Unit);
    }

    public static Product ToEntity(ProductCreateRequest request)
    {
        return new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };
    }

    public static void Apply(Product product, ProductUpdateRequest request)
    {
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
    }
}
