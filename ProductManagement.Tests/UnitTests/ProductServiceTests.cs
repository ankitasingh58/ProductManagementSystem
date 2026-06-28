using ProductManagement.Application.DTOs;
using ProductManagement.Application.Services;
using ProductManagement.Infrastructure.Repository;

namespace ProductManagement.Tests.UnitTests;

public sealed class ProductServiceTests
{
    [Fact]
    public async Task CreateProductShouldReturnCreatedProduct()
    {
        var repository = new InMemoryProductRepository();
        var service = new ProductService(repository);

        var result = await service.CreateProductAsync(new ProductCreateRequest("Laptop", "Gaming laptop", 1499.99m));

        Assert.Equal("Laptop", result.Name);
        Assert.Equal(1499.99m, result.Price);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task CreateItemShouldLinkToParentProduct()
    {
        var repository = new InMemoryProductRepository();
        var service = new ProductService(repository);

        var product = await service.CreateProductAsync(new ProductCreateRequest("Phone", "Smartphone", 899m));
        var item = await service.CreateItemAsync(new ItemCreateRequest(product.Id, "SKU-001", 3, "pcs"));

        Assert.Equal(product.Id, item.ProductId);
        Assert.Equal("SKU-001", item.Sku);
        Assert.Equal(3, item.Quantity);
    }
}
