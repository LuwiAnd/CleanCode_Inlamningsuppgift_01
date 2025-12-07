namespace Inlämningsuppgift_1.Dto.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
