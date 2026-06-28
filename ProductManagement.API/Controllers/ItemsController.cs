using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Interfaces;

namespace ProductManagement.API.Controllers;

[ApiController]
[Route("api/v1/items")]
public sealed class ItemsController : ControllerBase
{
    private readonly IProductService _productService;

    public ItemsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem([FromBody] ItemCreateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _productService.CreateItemAsync(request, cancellationToken);
            return Created($"/api/v1/items/{item.Id}", item);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "The parent product was not found." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
