namespace ProductManagement.Application.DTOs;

public record ProductDto(Guid Id, string Name, string? Description, decimal Price, IReadOnlyList<ItemDto> Items);

public record ProductCreateRequest(string Name, string? Description, decimal Price);

public record ProductUpdateRequest(string Name, string? Description, decimal Price);

public record ItemDto(Guid Id, Guid ProductId, string Sku, int Quantity, string? Unit);

public record ItemCreateRequest(Guid ProductId, string Sku, int Quantity, string? Unit);
