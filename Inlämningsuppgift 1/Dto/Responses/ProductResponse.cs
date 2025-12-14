namespace Inlämningsuppgift_1.Dto.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int StockBalance { get; set; }
    }
}
