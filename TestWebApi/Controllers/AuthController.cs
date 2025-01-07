using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebApi.IServices;
using TestWebApi.Models;
using TestWebApi.Models.RequestModels;
using TestWebApi.Models.ResponseModels;
using TestWebApi.Services;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var result = await _authService.Register(request);
                if (!result.IsSuccess)
                {
                    return BadRequest(new ApiResponse<object>(false, result.Message, null));
                }
                return Ok(new ApiResponse<object>(true, result.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, $"Internal server error: {ex.Message}", null));
            }
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _authService.Login(request);
                if (!result.IsSuccess)
                {
                    return BadRequest(new ApiResponse<object>(false, result.Message, null));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, $"Internal server error: {ex.Message}", null));
            }
        }
    }
}
