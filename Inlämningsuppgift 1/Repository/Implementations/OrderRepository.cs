using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;
using System.Xml.Linq;

namespace Inlämningsuppgift_1.Repository.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private static readonly List<Order> _orders = new();
        private static int _nextId = 1;

        public Order CreateOrder(Order order)
        {
            order.Id = _nextId++;
            _orders.Add(order);
            return order;
        }

        public Order? GetOrderById(int orderId)
        {
            return _orders.FirstOrDefault(o => o.Id == orderId);
        }

        public IEnumerable<Order> GetOrdersForUser(int userId)
        {
            return _orders.Where(o => o.UserId == userId).ToList();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orders.ToList();
        }

        public void UpdateOrder(Order order)
        {
            var existing = GetOrderById(order.Id);
            if (existing != null) _orders.Remove(existing);

            _orders.Add(order);
        }

        public void DeleteOrder(int orderId)
        {
            _orders.RemoveAll(o => o.Id == orderId);
        }
    }
}
