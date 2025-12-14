using Inlämningsuppgift_1.Dto.Responses;
using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;
using Inlämningsuppgift_1.Services.Interfaces;

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

        // Denna funktion kanske borde legat i en ny mappningsklass för att 
        // detta kan ses som ett nytt ansvar och därför göra att OrderService
        // bryter mot SRP. Det kändes dock lite onödigt att lägga den i en 
        // egen funktion, eftersom jag tänker att den endast ska användas 
        // tillsammans med denna klass.
        public OrderResponse ToOrderResponse(Order order)
        {
            return new OrderResponse
            {
                OrderId = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                Total = order.Total,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList()
            };
        }


        public Order? CreateOrderFromCart(int userId, Cart cart)
        {
            var items = cart.CartItems.Select(ci =>
            {
                var p = _productService.GetProductById(ci.ProductId);
                if (p == null) return null;

                return new OrderItem
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = p.Price
                };
            })
            .Where(i => i != null)
            .Cast<OrderItem>()
            .ToList();

            if (items.Count == 0)
                return null;

            return CreateOrder(userId, items);
        }

    }
}
