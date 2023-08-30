using AuthServer.API.Controllers.Common;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : BaseController
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
    {
        var result = await _productService.CreateProductAsync(productDto);
        return ActionResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductDetail([FromRoute] Guid id)
    {
        var result = await _productService.GetProductDetailAsync(id);
        return ActionResult(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllProductsByUser()
    {
        var result = await _productService.GetAllProductsByUserAsync();
        return ActionResult(result);
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateUserProduct([FromRoute]Guid id, [FromBody] UpdatePatchProductDto productDto)
    {
        var result = await _productService.UpdateUserProductAsync(id, productDto);
        return ActionResult(result);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> BulkDeleteProducts()
    {
        var result = await _productService.BulkDeleteProductsByUserAsync();
        return ActionResult(result);
    }
    
}