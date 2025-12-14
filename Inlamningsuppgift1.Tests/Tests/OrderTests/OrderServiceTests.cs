using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Services.Implementations;
using Inlämningsuppgift_1.Services.Interfaces;
using Inlamningsuppgift1.Tests.Fakes;

using Xunit;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamningsuppgift1.Tests.Tests.OrderTests
{
    public class OrderServiceTests
    {
        // --------------------------------------------------------------------
        // 1. CalculateOrderTotal räknar korrekt summa
        // --------------------------------------------------------------------
        [Fact]
        public void CalculateOrderTotal_ShouldReturnCorrectSum()
        {
            // ARRANGE
            var repo = new FakeOrderRepository();
            var mockProduct = new Mock<IProductService>();

            mockProduct.Setup(p => p.GetProductById(1))
                       .Returns(new Product { Id = 1, Price = 10m });

            mockProduct.Setup(p => p.GetProductById(2))
                       .Returns(new Product { Id = 2, Price = 5m });

            var service = new OrderService(repo, mockProduct.Object);

            var order = new Order
            {
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 2 }, // 2 * 10 = 20
                    new OrderItem { ProductId = 2, Quantity = 3 }  // 3 * 5 = 15
                }
            };

            // ACT
            var total = service.CalculateOrderTotal(order);

            // ASSERT
            Assert.Equal(35m, total);
        }


        // --------------------------------------------------------------------
        // 2. CreateOrderFromCart skapar order korrekt
        // --------------------------------------------------------------------
        [Fact]
        public void CreateOrderFromCart_ShouldCreateOrderCorrectly()
        {
            // ARRANGE
            var repo = new FakeOrderRepository();
            var mockProduct = new Mock<IProductService>();

            mockProduct.Setup(p => p.GetProductById(1))
                       .Returns(new Product { Id = 1, Name = "Pen", Price = 10m });

            var service = new OrderService(repo, mockProduct.Object);

            var cart = new Cart
            {
                UserId = 7,
                CartItems = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 3 }
                }
            };

            // ACT
            var order = service.CreateOrderFromCart(7, cart);

            // ASSERT
            Assert.NotNull(order);
            Assert.Equal(7, order!.UserId);
            Assert.Single(order.Items);
            Assert.Equal(30m, order.Total); // 3 * 10
        }


        // --------------------------------------------------------------------
        // 3. CreateOrderFromCart returnerar null vid tom cart
        // --------------------------------------------------------------------
        [Fact]
        public void CreateOrderFromCart_ShouldReturnNull_WhenCartIsEmpty()
        {
            // ARRANGE
            var repo = new FakeOrderRepository();
            var mockProduct = new Mock<IProductService>();
            var service = new OrderService(repo, mockProduct.Object);

            var cart = new Cart
            {
                UserId = 3,
                CartItems = new List<CartItem>()
            };

            // ACT
            var order = service.CreateOrderFromCart(3, cart);

            // ASSERT
            Assert.Null(order);
        }
    }
}
