using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Repository.Interfaces
{
    public interface IOrderRepository
    {
        public Order CreateOrder(Order order);
        public Order? GetOrderById(int orderId);
        public IEnumerable<Order> GetOrdersForUser(int userId);
        public IEnumerable<Order> GetAllOrders();
        public void UpdateOrder(Order order);
        public void DeleteOrder(int orderId);
    }
}
