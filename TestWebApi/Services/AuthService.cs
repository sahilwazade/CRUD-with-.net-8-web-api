using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestWebApi.IServices;
using TestWebApi.Models;
using TestWebApi.Models.RequestModels;
using TestWebApi.Models.ResponseModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestWebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<GenericResponse> Register(RegisterRequest request)
        {
            var user = new IdentityUser()
            {
                UserName =  request.Email,
                Email = request.Email,
            };

            try
            {
                var createUser = await _userManager.CreateAsync(user, request.Password);
                var errors = createUser.Errors.Select(e => e.Description).ToArray();
                if (!createUser.Succeeded)
                {
                    return new GenericResponse()
                    {
                        IsSuccess = false,
                        Message = errors.ToString()
                    };
                }
                else
                {
                    return new GenericResponse()
                    {
                        IsSuccess = true,
                        Message = "User created successfully."
                    };
                }
            }
            catch(Exception ex) 
            {
                return new GenericResponse()
                {
                    IsSuccess = false,
                    Message = $"Error : { ex.Message }"
                };
            }
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return new AuthResponse()
                    {
                        IsSuccess = false,
                        Message = user == null ? "User not found." : "Password dosent match."
                    };
                }

                var token = GenerateJwtToken(user);
                if (!token.IsSuccess)
                {
                    return new AuthResponse()
                    {
                        IsSuccess = false,
                        Message = token.Message,
                    };
                }

                return new AuthResponse()
                {
                    IsSuccess = true,
                    Message = $"{request.Email} user logIn successfully",
                    Expiration = token.Expiration,
                    Token = token.Token,
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse()
                {
                    IsSuccess = false,
                    Message = $" Error : { ex.Message }"
                };
            }
        }

        private AuthResponse GenerateJwtToken(IdentityUser user)
        {
            try
            {
                if (user.Email == null)
                {
                    return new AuthResponse()
                    {
                        IsSuccess = false,
                        Message = "Email is empty." 
                    };
                }

                if (!double.TryParse(_config["Jwt:AccessTokenDuration"], out double tokenDuration))
                {
                    return new AuthResponse()
                    {
                        IsSuccess = false,
                        Message = "Invalid AccessTokenDuration configuration."
                    };
                }

                var claims = new[]
                {
                    new Claim("email", user.Email),
                    new Claim(JwtRegisteredClaimNames.Nbf,
                                new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp,
                                new DateTimeOffset(
                                    DateTime.Now.AddMinutes(tokenDuration)).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, _config["Jwt:Issuer"])
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:JwtSecret"].Trim()));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                     new JwtHeader(signingCredentials),
                     new JwtPayload(claims)
                );

                var jsonToken = new JwtSecurityTokenHandler().WriteToken(token);

                if(jsonToken != null)
                {
                    return new AuthResponse()
                    {
                        Token = jsonToken,
                        IsSuccess = true
                    };
                }
                else
                {
                    return new AuthResponse()
                    {
                        IsSuccess = false
                    };
                }
            }
            catch( Exception ex)
            {
                return new AuthResponse()
                {
                    IsSuccess = false,
                    Message = $"Error : {ex.Message}"
                };
            }

            //try
            //{
            //    if( user.Email == null || user.Id == null)
            //    {
            //        return new AuthResponse()
            //        {
            //             IsSuccess = false,
            //             Message = user.Email == null ? "Email is empty." : "Id is empty."
            //        };
            //    }

            //    var claims = new[]
            //    {
            //        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim(ClaimTypes.NameIdentifier, user.Id)
            //    };

            //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            //    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //    var token = new JwtSecurityToken(
            //        issuer: _config["Jwt:Issuer"],
            //        audience: _config["Jwt:Audiance"],
            //        claims: claims,
            //        expires: DateTime.Now.AddMinutes(60),
            //        signingCredentials: cred
            //    );

            //    return new AuthResponse()
            //    {
            //        Token = new JwtSecurityTokenHandler().WriteToken(token),
            //        Expiration = token.ValidTo,
            //        IsSuccess = true
            //    };
            //}
            //catch (Exception ex)
            //{
            //    return new AuthResponse()
            //    {
            //        IsSuccess = false,
            //        Message = $"Error : { ex.Message }"
            //    };
            //}
        }
    }
}
