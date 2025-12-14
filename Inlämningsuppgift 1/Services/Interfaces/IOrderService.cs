using Inlämningsuppgift_1.Dto.Responses;
using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Services.Interfaces
{
    public interface IOrderService
    {
        public Order CreateOrder(int userId, List<OrderItem> items);
        public Order? GetOrderById(int orderId);
        public IEnumerable<Order> GetOrdersForUser(int userId);
        public decimal CalculateOrderTotal(Order order);

        public OrderResponse ToOrderResponse(Order order);
        public Order? CreateOrderFromCart(int userId, Cart cart);
    }
}
