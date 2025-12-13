namespace Inlämningsuppgift_1.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
