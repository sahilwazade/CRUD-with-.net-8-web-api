namespace TestWebApi.Models.ResponseModels
{
    public class GetProductByIdResponse : GenericResponse
    {
        public Product product { get; set; }
    }
}