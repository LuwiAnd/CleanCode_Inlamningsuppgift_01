using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;
using Inlämningsuppgift_1.Services.Implementations;
using Inlämningsuppgift_1.Services.Interfaces;
using Inlamningsuppgift1.Tests.Fakes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Inlamningsuppgift1.Tests.Tests.CartTests
{
    public class CartServiceTests
    {
        // --------------------------------------------------------------------
        // 1. AddToCart skapar ny cart om ingen finns  (MOCK)
        // --------------------------------------------------------------------
        [Fact]
        public void AddToCart_ShouldCreateNewCart_WhenNoneExists()
        {
            // ARRANGE
            var mockRepo = new Mock<ICartRepository>();
            mockRepo
                .Setup(r => r.GetCartByUserId(It.IsAny<int>()))
                .Returns((Cart?)null);

            var mockProductService = new Mock<IProductService>();

            //var service = new CartService(mockRepo.Object);
            var service = new CartService(mockRepo.Object, mockProductService.Object);


            // ACT
            service.AddToCart(userId: 5, productId: 10, quantity: 2);

            // ASSERT
            mockRepo.Verify(r => r.CreateCart(It.IsAny<Cart>()), Times.Once);
        }



        // --------------------------------------------------------------------
        // 2. AddToCart ökar quantity om produkten redan finns  (FAKE)
        // --------------------------------------------------------------------
        [Fact]
        public void AddToCart_ShouldIncreaseQuantity_WhenProductAlreadyExists()
        {
            // ARRANGE – FAKE repository
            var repo = new FakeCartRepository();

            // Befintlig cart
            repo.CreateCart(new Cart
            {
                UserId = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem { ProductId = 10, Quantity = 2 }
                }
            });

            var mockProductService = new Mock<IProductService>();
            var service = new CartService(repo, mockProductService.Object);

            // ACT – lägg samma produkt igen
            service.AddToCart(1, productId: 10, quantity: 3);

            var cart = repo.GetCartByUserId(1);

            // ASSERT
            Assert.NotNull(cart);
            Assert.Single(cart!.CartItems);
            Assert.Equal(5, cart.CartItems[0].Quantity);
        }



        // --------------------------------------------------------------------
        // 3. RemoveFromCart tar bort rätt produkt  (FAKE)
        // --------------------------------------------------------------------
        [Fact]
        public void RemoveFromCart_ShouldRemoveCorrectProduct()
        {
            // ARRANGE – FAKE repo
            var repo = new FakeCartRepository();

            repo.CreateCart(new Cart
            {
                UserId = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem { ProductId = 5, Quantity = 1 },
                    new CartItem { ProductId = 10, Quantity = 2 }
                }
            });

            var mockProductService = new Mock<IProductService>();
            var service = new CartService(repo, mockProductService.Object);

            // ACT
            service.RemoveFromCart(1, productId: 10);

            var cart = repo.GetCartByUserId(1);

            // ASSERT
            Assert.NotNull(cart);
            Assert.Single(cart!.CartItems);
            Assert.Equal(5, cart.CartItems[0].ProductId);
        }



        // --------------------------------------------------------------------
        // 4. ClearCart tar bort kundvagnen  (MOCK)
        // --------------------------------------------------------------------
        [Fact]
        public void ClearCart_ShouldCallDeleteCart()
        {
            // ARRANGE
            var mockRepo = new Mock<ICartRepository>();
            var mockProductService = new Mock<IProductService>();

            var service = new CartService(mockRepo.Object, mockProductService.Object);

            // ACT
            service.ClearCart(3);

            // ASSERT
            mockRepo.Verify(r => r.DeleteCart(3), Times.Once);
        }
    }
}
