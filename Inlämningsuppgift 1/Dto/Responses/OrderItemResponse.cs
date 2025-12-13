namespace Inlämningsuppgift_1.Dto.Responses
{
    public class OrderItemResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
