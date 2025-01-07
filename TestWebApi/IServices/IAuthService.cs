using Microsoft.AspNetCore.Identity;
using TestWebApi.Models;
using TestWebApi.Models.RequestModels;
using TestWebApi.Models.ResponseModels;

namespace TestWebApi.IServices
{
    public interface IAuthService
    {
        Task<GenericResponse> Register(RegisterRequest request);
        Task<AuthResponse> Login(LoginRequest request);
    }
}
