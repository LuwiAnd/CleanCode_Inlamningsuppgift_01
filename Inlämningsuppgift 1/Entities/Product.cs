namespace Inlämningsuppgift_1.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int StockBalance { get; set; }
        public decimal Price { get; set; }
    }
}