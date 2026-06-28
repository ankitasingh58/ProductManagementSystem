using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDto?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateProductAsync(ProductCreateRequest request, CancellationToken cancellationToken = default);
    Task<ProductDto?> UpdateProductAsync(Guid id, ProductUpdateRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ItemDto>> GetItemsForProductAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<ItemDto> CreateItemAsync(ItemCreateRequest request, CancellationToken cancellationToken = default);
}
