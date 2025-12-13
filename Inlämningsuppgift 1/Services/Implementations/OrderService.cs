using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Services.Interfaces;
using Inlämningsuppgift_1.Repository.Interfaces;

namespace Inlämningsuppgift_1.Services.Implementations
{
    public class OrderService : IOrderService
    {
        //private static readonly List<Order> Orders = new List<Order>();
        //private static int _nextId = 1;

        private readonly IOrderRepository _repository;
        private readonly IProductService _productService;

        public OrderService(IOrderRepository repository, IProductService productService)
        {
            _repository = repository;
            _productService = productService;
        }


        

        public Order CreateOrder(int userId, List<OrderItem> items)
        {
            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Items = items,
                Total = CalculateOrderTotalFromItems(items)
            };

            return _repository.CreateOrder(order);
        }

        public Order? GetOrderById(int orderId) => _repository.GetOrderById(orderId);

        public IEnumerable<Order> GetOrdersForUser(int userId) => _repository.GetOrdersForUser(userId);

        // ---- Nya funktioner av Luwi ----

        public decimal CalculateOrderTotal(Order order)
            => CalculateOrderTotalFromItems(order.Items);

        private decimal CalculateOrderTotalFromItems(List<OrderItem> items)
        {
            // Här kanske jag borde lägga till en kontroll att det finns priser 
            // för alla produkter.
            return items.Sum(i =>
            {
                var p = _productService.GetProductById(i.ProductId);
                return (p?.Price ?? 0) * i.Quantity;
            });
        }

        
    }
}
