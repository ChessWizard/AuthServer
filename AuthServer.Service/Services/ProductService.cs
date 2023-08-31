using System.Net;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using AuthServer.Core.UnitofWork;
using AuthServer.Data.ContextAccessor;
using AuthServer.Service.Mapping;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using SharedLibrary.Extensions;

namespace AuthServer.Service.Services;

public class ProductService : IProductService
{
    private readonly IUnitofWork _unitofWork;
    private readonly ISecurityContextAccessor _contextAccessor;
    private readonly UserManager<UserApp> _userManager;

    public ProductService(IUnitofWork unitofWork, ISecurityContextAccessor contextAccessor, UserManager<UserApp> userManager)
    {
        _unitofWork = unitofWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    /// <summary>
    /// Kullanýcýya ait bir ürün yaratmaya yarar
    /// </summary>
    /// <param name="productDto"></param>
    /// <returns></returns>
    public async Task<Response<NoDataDto>> CreateProductAsync(CreateProductDto productDto)
    {
        var user = await _userManager.FindByIdAsync(_contextAccessor.UserId.ToString());

        if (user is null)
            return Response<NoDataDto>.Error("User not found!", (int)HttpStatusCode.NotFound);

        Product product = new()
        {
            UserApp = user,
            Name = productDto.Name,
            Price = productDto.Price,
            Stock = productDto.Stock,
        };

        await _unitofWork.GetRepository<Product>()
            .AddAsync(product);

        await _unitofWork.SaveChangesAsync();

        return Response<NoDataDto>.Success((int)HttpStatusCode.Created);
    }

    /// <summary>
    /// Kullanýcýya ait ürünleri getirmeye yarar
    /// </summary>
    /// <returns></returns>
    public async Task<Response<List<ProductDto>>> GetAllProductsByUserAsync()
    {
        var user = await _userManager.Users
            .Include(x => x.Products.Where(x => !x.IsDeleted))
            .FirstOrDefaultAsync(x => x.Id == _contextAccessor.UserId);
        
        if (user is null)
            return Response<List<ProductDto>>.Error("User not found!", (int)HttpStatusCode.NotFound);

        var products = user.Products
                           .ToList();
        if(products.IsNullOrNotAny())
            return Response<List<ProductDto>>.Error("Products not found!", (int)HttpStatusCode.NotFound);

        var productsDto = ObjectMapper.Mapper.Map<List<ProductDto>>(products);
        
        return Response<List<ProductDto>>.Success(productsDto, (int)HttpStatusCode.OK);
    }

    /// <summary>
    /// Ýlgili ürünün detayýný getirmeye yarar
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Response<ProductDto>> GetProductDetailAsync(Guid id)
    {
        var product = await _unitofWork.GetRepository<Product>()
            .GetAsync(x => x.Id == id);

        if(product is null)
            return Response<ProductDto>.Error("Product not found!", (int)HttpStatusCode.NotFound);

        var productDto = ObjectMapper.Mapper.Map<ProductDto>(product);
        return Response<ProductDto>.Success(productDto, (int)HttpStatusCode.OK);
    }

    #region Patch Update Product

    /// <summary>
    /// Patch þekilde product güncelleme iþlemi
    /// </summary>
    /// <param name="id"></param>
    /// <param name="productDto"></param>
    /// <returns></returns>
    public async Task<Response<NoDataDto>> UpdateUserProductAsync(Guid id, UpdatePatchProductDto productDto)
    {
        var product = await _unitofWork.GetRepository<Product>()
            .GetAsync(x => x.Id == id && x.UserAppId == _contextAccessor.UserId);

        if (product is null)
            return Response<NoDataDto>.Error("Product not found!", (int)HttpStatusCode.NotFound);

        ProductNameUpdate(product, productDto);
        ProductPriceUpdate(product, productDto);
        ProductArchiveStatusUpdate(product, productDto);
        ProductStockUpdate(product, productDto);

        _unitofWork.GetRepository<Product>()
            .Update(product);

        await _unitofWork.SaveChangesAsync();
        return Response<NoDataDto>.Success((int)HttpStatusCode.NoContent);
    }

    private void ProductNameUpdate(Product product, UpdatePatchProductDto productDto)
    {
        if (!string.IsNullOrEmpty(productDto.Name))
            product.Name = productDto.Name;
    }

    private void ProductPriceUpdate(Product product, UpdatePatchProductDto productDto)
    {
        if (productDto.Price.HasValue)
            product.Price = productDto.Price.Value;
    }

    private void ProductArchiveStatusUpdate(Product product, UpdatePatchProductDto productDto)
    {
        if (productDto.IsDeleted.HasValue)
            product.IsDeleted = productDto.IsDeleted.Value;
    }

    private void ProductStockUpdate(Product product, UpdatePatchProductDto productDto)
    {
        if (productDto.Stock.HasValue)
            product.Stock = productDto.Stock.Value;
    }

    #endregion

    /// <summary>
    /// Kullanýcý ürünlerinin toplu þekilde Hard Delete edilmesi
    /// </summary>
    /// <returns></returns>
    public async Task<Response<NoDataDto>> BulkDeleteProductsByUserAsync()
    {
        var user = await _userManager.Users
            .Include(x => x.Products.Where(x => !x.IsDeleted))
            .FirstOrDefaultAsync(x => x.Id == _contextAccessor.UserId);

        if (user is null)
            return Response<NoDataDto>.Error("User not found!", (int)HttpStatusCode.NotFound);

        var products = user.Products
                           .ToList();
        if (products.IsNullOrNotAny())
            return Response<NoDataDto>.Error("Products not found!", (int)HttpStatusCode.NotFound);

        _unitofWork.GetRepository<Product>()
            .RemoveRange(products);

        await _unitofWork.SaveChangesAsync();
        return Response<NoDataDto>.Success((int)HttpStatusCode.NoContent);
    }
}