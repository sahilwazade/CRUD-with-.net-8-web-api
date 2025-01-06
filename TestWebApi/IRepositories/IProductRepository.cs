using TestWebApi.Models.ResponseModels;
using TestWebApi.Models;
using TestWebApi.Models.RequestModels;

namespace TestWebApi.IRepositories
{
    public interface IProductRepository
    {
        Task<GetProductResponse> GetProduct();
        Task<GetProductByIdResponse> GetProductById(int id);
        Task<GenericResponse> AddProduct(AddProductRequest request);
        Task<GenericResponse> UpdateProduct(UpdateProductRequest request);
        Task<GenericResponse> DeleteProduct(int id);
    }
}
