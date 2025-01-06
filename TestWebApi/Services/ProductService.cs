using TestWebApi.IRepositories;
using TestWebApi.IServices;
using TestWebApi.Models.ResponseModels;
using TestWebApi.Models;
using TestWebApi.Models.RequestModels;

namespace TestWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetProductResponse> GetProduct()
        {
            return await _productRepository.GetProduct();
        }

        public async Task<GetProductByIdResponse> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task<GenericResponse> AddProduct(AddProductRequest request)
        {
            return await _productRepository.AddProduct(request);
        }

        public async Task<GenericResponse> UpdateProduct(UpdateProductRequest request)
        {
            return await _productRepository.UpdateProduct(request);
        }

        public async Task<GenericResponse> DeleteProduct(int id)
        {
            return await _productRepository.DeleteProduct(id);
        }
    }
}
