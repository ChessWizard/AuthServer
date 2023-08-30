using AuthServer.Core.Dtos;
using SharedLibrary.Dtos;

namespace AuthServer.Core.Services;

public interface IProductService
{
    public Task<Response<NoDataDto>> CreateProductAsync(CreateProductDto productDto);

    public Task<Response<List<ProductDto>>> GetAllProductsByUserAsync();

    public Task<Response<ProductDto>> GetProductDetailAsync(Guid id);

    public Task<Response<NoDataDto>> UpdateUserProductAsync(Guid id, UpdatePatchProductDto productDto);

    public Task<Response<NoDataDto>> BulkDeleteProductsByUserAsync();
}