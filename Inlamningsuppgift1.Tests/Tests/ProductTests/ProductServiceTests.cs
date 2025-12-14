using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Services.Implementations;
using Inlamningsuppgift1.Tests.Fakes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Inlamningsuppgift1.Tests.Tests.ProductTests
{
    public class ProductServiceTests
    {
        // ------------------------------------------------
        // Test 1. CreateProduct skapar produkt korrekt
        // ------------------------------------------------
        [Fact]
        public void CreateProduct_ShouldAssignCorrectValues()
        {
            // Arrange
            var repo = new FakeProductRepository();
            var service = new ProductService(repo);

            // Act
            var result = service.CreateProduct("Pen", 10m, 5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pen", result.Name);
            Assert.Equal(10m, result.Price);
            Assert.Equal(5, result.StockBalance);
        }

        // ------------------------------------------------
        // Slut på:
        // Test 1. CreateProduct skapar produkt korrekt
        // ------------------------------------------------


        // ---------------------------------------------------------------------
        // Test 2. CreateProduct kastar ArgumentException vid ogiltiga värden.
        // ---------------------------------------------------------------------
        [Fact]
        public void CreateProduct_ShouldThrow_WhenNameIsEmpty()
        {
            var repo = new FakeProductRepository();
            var service = new ProductService(repo);

            Assert.Throws<ArgumentException>(() => service.CreateProduct("", 10m, 5));
        }

        [Fact]
        public void CreateProduct_ShouldThrow_WhenPriceIsNegative()
        {
            var repo = new FakeProductRepository();
            var service = new ProductService(repo);

            Assert.Throws<ArgumentException>(() => service.CreateProduct("Pen", -1m, 5));
        }

        [Fact]
        public void CreateProduct_ShouldThrow_WhenStockIsNegative()
        {
            var repo = new FakeProductRepository();
            var service = new ProductService(repo);

            Assert.Throws<ArgumentException>(() => service.CreateProduct("Pen", 10m, -5));
        }

        // ---------------------------------------------------------------------
        // Slut på:
        // Test 2. CreateProduct kastar ArgumentException vid ogiltiga värden.
        // ---------------------------------------------------------------------



        // ------------------------------------------------
        // Test 3. Search returnerar rätt produkter.
        // ------------------------------------------------

        [Fact]
        public void Search_ShouldReturnMatchingProducts_ByName()
        {
            var repo = new FakeProductRepository();
            repo.CreateProduct(new Product { Name = "Pen", Price = 10m, StockBalance = 1 });
            repo.CreateProduct(new Product { Name = "Pencil", Price = 12m, StockBalance = 1 });
            repo.CreateProduct(new Product { Name = "Notebook", Price = 20m, StockBalance = 1 });

            var service = new ProductService(repo);

            var results = service.Search("pen");

            Assert.Equal(2, results.Count()); // Pen + Pencil
        }

        [Fact]
        public void Search_ShouldReturnMatchingProducts_ByPrice()
        {
            var repo = new FakeProductRepository();
            repo.CreateProduct(new Product { Name = "Cheap", Price = 5m, StockBalance = 1 });
            repo.CreateProduct(new Product { Name = "Expensive", Price = 50m, StockBalance = 1 });

            var service = new ProductService(repo);

            var results = service.Search("5"); // matches price "5"

            Assert.Equal(2, results.Count());
        }

        [Fact]
        public void Search_ShouldReturnAllProducts_WhenQueryIsNullOrEmpty()
        {
            var repo = new FakeProductRepository();
            repo.CreateProduct(new Product { Name = "A", Price = 1m, StockBalance = 1 });
            repo.CreateProduct(new Product { Name = "B", Price = 2m, StockBalance = 1 });

            var service = new ProductService(repo);

            var resultsNull = service.Search(null);
            var resultsEmpty = service.Search("");

            Assert.Equal(2, resultsNull.Count());
            Assert.Equal(2, resultsEmpty.Count());
        }


        // ------------------------------------------------
        // Slut på:
        // Test 3. Search returnerar rätt produkter.
        // ------------------------------------------------


        // ------------------------------------------------
        // Test 4. ChangeProductStock uppdaterar antal i lager.
        // ------------------------------------------------

        [Fact]
        public void ChangeProductStock_ShouldIncreaseStock()
        {
            var repo = new FakeProductRepository();
            repo.CreateProduct(new Product { Name = "Pen", Price = 10m, StockBalance = 5 });

            var service = new ProductService(repo);

            var ok = service.ChangeProductStock(1, 3);

            Assert.True(ok);
            Assert.Equal(8, repo.GetProductById(1)!.StockBalance);
        }

        [Fact]
        public void ChangeProductStock_ShouldDecreaseStock()
        {
            var repo = new FakeProductRepository();
            repo.CreateProduct(new Product { Name = "Pen", Price = 10m, StockBalance = 5 });

            var service = new ProductService(repo);

            var ok = service.ChangeProductStock(1, -2);

            Assert.True(ok);
            Assert.Equal(3, repo.GetProductById(1)!.StockBalance);
        }

        [Fact]
        public void ChangeProductStock_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            var repo = new FakeProductRepository();
            var service = new ProductService(repo);

            var ok = service.ChangeProductStock(999, 5);

            Assert.False(ok);
        }


        // ------------------------------------------------
        // Slut på:
        // Test 4. ChangeProductStock uppdaterar antal i lager.
        // ------------------------------------------------


    }
}
