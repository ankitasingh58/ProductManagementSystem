using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Infrastructure.Repository;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> _products = new();
    private readonly Dictionary<Guid, Item> _items = new();

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = _products.Values.Select(Hydrate).OrderBy(product => product.Name).ToList();
        return Task.FromResult<IReadOnlyList<Product>>(products);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (_products.TryGetValue(id, out var product))
        {
            return Task.FromResult<Product?>(Hydrate(product));
        }

        return Task.FromResult<Product?>(null);
    }

    public Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _products[product.Id] = product;
        return Task.FromResult(Hydrate(product));
    }

    public Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        if (!_products.ContainsKey(product.Id))
        {
            return Task.FromResult<Product?>(null);
        }

        _products[product.Id] = product;
        return Task.FromResult<Product?>(Hydrate(product));
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var productRemoved = _products.Remove(id);
        if (productRemoved)
        {
            var remainingItems = _items.Where(item => item.Value.ProductId != id).ToDictionary(item => item.Key, item => item.Value);
            _items.Clear();
            foreach (var item in remainingItems)
            {
                _items[item.Key] = item.Value;
            }
        }

        return Task.FromResult(productRemoved);
    }

    public Task<IReadOnlyList<Item>> GetItemsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var items = _items.Values.Where(item => item.ProductId == productId).OrderBy(item => item.Sku).ToList();
        return Task.FromResult<IReadOnlyList<Item>>(items);
    }

    public Task<Item> AddItemAsync(Item item, CancellationToken cancellationToken = default)
    {
        _items[item.Id] = item;

        if (_products.TryGetValue(item.ProductId, out var product))
        {
            if (!product.Items.Any(existingItem => existingItem.Id == item.Id))
            {
                product.Items.Add(item);
            }
        }

        return Task.FromResult(item);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_products.ContainsKey(id));
    }

    private Product Hydrate(Product product)
    {
        product.Items = _items.Values.Where(item => item.ProductId == product.Id).ToList();
        return product;
    }
}
