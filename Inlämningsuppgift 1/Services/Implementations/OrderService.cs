namespace Inlämningsuppgift_1.Services.Implementations
{
    public class OrderService
    {
        private static readonly List<Order> Orders = new List<Order>();
        private static int _nextId = 1;

        public class Order
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public DateTime CreatedAt { get; set; }
            public decimal Total { get; set; }
            public List<object> Items { get; set; } = new List<object>(); 
        }

     
        public Order CreateOrder(int userId, List<object> items, decimal total)
        {
            var o = new Order
            {
                Id = _nextId++,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Items = items,
                Total = total
            };
            Orders.Add(o);
            return o;
        }

        public Order? Get(int orderId) => Orders.FirstOrDefault(o => o.Id == orderId);

        public IEnumerable<Order> GetForUser(int userId) => Orders.Where(o => o.UserId == userId);
    }
}
