namespace Inlämningsuppgift_1.Dto.Responses
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItemResponse> Items { get; set; } = new();
        public decimal Total { get; set; }

    }
}
