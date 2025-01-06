namespace TestWebApi.Models.ResponseModels
{
    public class GetProductResponse : GenericResponse
    {
        public List<Product> products { get; set; } = new List<Product>();
    }
}
