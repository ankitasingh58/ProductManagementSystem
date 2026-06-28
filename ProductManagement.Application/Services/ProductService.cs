using ProductManagement.Application.DTOs;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Mapping;
using ProductManagement.Application.Validators;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ProductValidator _productValidator = new();
    private readonly ItemValidator _itemValidator = new();

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _repository.GetAllAsync(cancellationToken);
        return products.Select(ProductMapper.ToDto).ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        return product is null ? null : ProductMapper.ToDto(product);
    }

    public async Task<ProductDto> CreateProductAsync(ProductCreateRequest request, CancellationToken cancellationToken = default)
    {
        _productValidator.Validate(request.Name, request.Price);

        var product = ProductMapper.ToEntity(request);
        var createdProduct = await _repository.AddAsync(product, cancellationToken);
        return ProductMapper.ToDto(createdProduct);
    }

    public async Task<ProductDto?> UpdateProductAsync(Guid id, ProductUpdateRequest request, CancellationToken cancellationToken = default)
    {
        _productValidator.Validate(request.Name, request.Price);

        var existingProduct = await _repository.GetByIdAsync(id, cancellationToken);
        if (existingProduct is null)
        {
            return null;
        }

        ProductMapper.Apply(existingProduct, request);
        var updatedProduct = await _repository.UpdateAsync(existingProduct, cancellationToken);
        return updatedProduct is null ? null : ProductMapper.ToDto(updatedProduct);
    }

    public async Task<bool> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<ItemDto>> GetItemsForProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        if (!await _repository.ExistsAsync(productId, cancellationToken))
        {
            throw new KeyNotFoundException($"Product {productId} was not found.");
        }

        var items = await _repository.GetItemsByProductIdAsync(productId, cancellationToken);
        return items.Select(ProductMapper.ToItemDto).ToList();
    }

    public async Task<ItemDto> CreateItemAsync(ItemCreateRequest request, CancellationToken cancellationToken = default)
    {
        _itemValidator.Validate(request.Sku, request.Quantity);

        if (!await _repository.ExistsAsync(request.ProductId, cancellationToken))
        {
            throw new KeyNotFoundException($"Product {request.ProductId} was not found.");
        }

        var item = new Item
        {
            ProductId = request.ProductId,
            Sku = request.Sku,
            Quantity = request.Quantity,
            Unit = request.Unit
        };

        var createdItem = await _repository.AddItemAsync(item, cancellationToken);
        return ProductMapper.ToItemDto(createdItem);
    }
}
