using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebApi.IServices;
using TestWebApi.Models;
using TestWebApi.Models.RequestModels;
using TestWebApi.Models.ResponseModels;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProduct()
        {
            try
            {
                var result = await _productService.GetProduct();
                if (result == null || result.IsSuccess == false)
                {
                    return BadRequest(new ApiResponse<object>(false, result.Message, null));
                }
                return Ok(new ApiResponse<List<Product>>(true, result.Message, result.products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, $"Internal server error: {ex.Message}", null));
            }
        }

        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var result = await _productService.GetProductById(id);
                if (result == null || result.IsSuccess == false)
                {
                    return BadRequest(new ApiResponse<object>(false, result.Message, null));
                }
                return Ok(new ApiResponse<Product>(true, result.Message, result.product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, $"Internal server error: {ex.Message}", null));
            }
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            try
            {
                var result = await _productService.AddProduct(request);
                if (!result.IsSuccess)
                {
                    return BadRequest(new ApiResponse<object>(false, result.Message, null));
                }
                return Ok(new ApiResponse<GenericResponse>(true, result.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, $"Internal server error: {ex.Message}", null));
            }
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequest request)
        {
            try
            {
                var result = await _productService.UpdateProduct(request);
                if (!result.IsSuccess)
                {
                    return BadRequest(new ApiResponse<object>(false, result.Message, null));
                }
                return Ok(new ApiResponse<GenericResponse>(true, result.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, $"Internal server error: {ex.Message}", null));
            }
        }

        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProduct(id);
                if (!result.IsSuccess)
                {
                    return BadRequest(new ApiResponse<object>(false, result.Message, null));
                }
                return Ok(new ApiResponse<GenericResponse>(true, result.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, $"Internal server error: {ex.Message}", null));
            }
        }
    }
}
