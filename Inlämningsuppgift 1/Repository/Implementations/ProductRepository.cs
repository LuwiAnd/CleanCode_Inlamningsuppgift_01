using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;

namespace Inlämningsuppgift_1.Repository.Implementations
{
    public class ProductRepository : IProductRepository
    {
        //private static readonly List<Product> _products = new List<Product>();
        private static readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Pen", Price = 1.5m, StockBalance = 100 },
            new Product { Id = 2, Name = "Notebook", Price = 3.0m, StockBalance = 50 },
            new Product { Id = 3, Name = "Mug", Price = 6.0m, StockBalance = 20 }
        };

        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products.ToList();
        }

        public void CreateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var newId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            
            // Här kanske jag ska lägga på ett slumpat heltal, eftersom
            // man inte vill att någon ska kunna ta reda på hur många
            // produkter man har i databasen och vilka Id de har.
            product.Id = newId; 


            _products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            var preexistingProduct = GetProductById(product.Id);
            if (preexistingProduct != null)
                _products.Remove(preexistingProduct);

            _products.Add(product);
        }

        public void DeleteProduct(int id)
        {
            _products.RemoveAll(p => p.Id == id);
        }

    }
}
