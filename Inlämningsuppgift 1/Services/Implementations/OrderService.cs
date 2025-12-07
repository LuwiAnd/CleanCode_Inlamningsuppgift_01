using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Services.Interfaces;

namespace Inlämningsuppgift_1.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private static readonly List<Order> Orders = new List<Order>();
        private static int _nextId = 1;

        

     
        public Order CreateOrder(int userId, List<object> items, decimal total)
        {
            var order = new Order
            {
                Id = _nextId++,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Items = items,
                Total = total
            };
            Orders.Add(order);
            return order;
        }

        public Order? Get(int orderId) => Orders.FirstOrDefault(o => o.Id == orderId);

        public IEnumerable<Order> GetForUser(int userId) => Orders.Where(o => o.UserId == userId);

        // ---- Nya funktioner av Luwi ----
        public decimal CalculateOrderTotal(Order order)
        {

        }

    }
}
