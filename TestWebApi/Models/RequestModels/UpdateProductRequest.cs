namespace TestWebApi.Models.RequestModels
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }
}