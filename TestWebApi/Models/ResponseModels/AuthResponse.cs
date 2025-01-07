namespace TestWebApi.Models.ResponseModels
{
    public class AuthResponse : GenericResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}