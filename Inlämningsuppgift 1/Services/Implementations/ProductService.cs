using Inlämningsuppgift_1.Repository.Interfaces;
using Inlämningsuppgift_1.Services.Interfaces;
using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }



        //public List<Product> GetAll() => _repository.GetAll();
        public IEnumerable<Product> GetAllProducts() => _repository.GetAll();

        public Product? GetProductById(int id) => _repository.GetProductById(id);

        //public IEnumerable<Product> Search(string? query)
        public List<Product> Search(string? query)
        {
            if (string.IsNullOrWhiteSpace(query)) return GetAllProducts();
            var q = query!.ToLowerInvariant();

            var products = GetAllProducts();

            return products
                .Where(
                    p => p.Name.ToLowerInvariant().Contains(q) 
                    || 
                    p.Price.ToString().Contains(q)
                )
                .ToList();
        }

        //public Product Create(string name, decimal price, int stock)
        //public void Create(Product product)
        public void CreateProduct(string name, decimal price, int stockBalance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(
                    nameof(name), 
                    "Invalid product name: name cannot be empty."
                );

            if (price < 0)
                throw new ArgumentException(
                    nameof(price), 
                    "Invalid price: price must be positive."
                );

            if (stockBalance < 0)
                throw new ArgumentException(
                    nameof(stockBalance), 
                    "Invalid stock balance: stock balance must be positive."
                );

            var products = _repository.GetAll();
            var newId = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            var product = new Product { Id = newId, Name = name, Price = price, StockBalance = stockBalance };

            _repository.CreateProduct(product);
        }

        public bool ChangeProductStock(int productId, int delta)
        {
            var product = GetProductById(productId);

            if (product == null) return false;
            
            product.StockBalance += delta;
            _repository.UpdateProduct(product);

            return true;
        }

        public void UpdateProduct(Product product)
        {
            var preexistingProduct = _repository.GetProductById(product.Id);
            if (preexistingProduct == null)
                throw new ArgumentException($"Product with ID {product.Id} does not exist.");

            _repository.UpdateProduct(product);
        }

        public void DeleteProduct(int id) => 
            _repository.DeleteProduct(id);
    }
}
