using Inlämningsuppgift_1.Repository.Interfaces;
using Inlämningsuppgift_1.Services.Interfaces;
using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Services.Implementations
{
    public class ProductService : IProductService
    {
        // Detta flyttar jag till ProductRepository.
        //private static readonly List<Product> Products = new List<Product>
        //{
        //    new Product { Id = 1, Name = "Pen", Price = 1.5m, Stock = 100 },
        //    new Product { Id = 2, Name = "Notebook", Price = 3.0m, Stock = 50 },
        //    new Product { Id = 3, Name = "Mug", Price = 6.0m, Stock = 20 }
        //};

        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }



        //public List<Product> GetAll() => _repository.GetAll();
        public IEnumerable<Product> GetAllProducts() => _repository.GetAll();

        public Product? GetProductById(int id) => _repository.GetProductById(id);

        //public List<Product> Search(string? query)
        public IEnumerable<Product> Search(string? query)
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
        public void Create(string name, decimal price, int stockBalance)
        {
            if (string.IsNullOrWhiteSpace(name) || price < 0 || stockBalance < 0)
                throw new ArgumentException("Cannot create a new product from null value.");

            var products = _repository.GetAll();
            var newId = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            var product = new Product { Id = newId, Name = name, Price = price, StockBalance = stockBalance };

            _repository.CreateProduct(product);
        }

        public bool ChangeStock(int id, int delta)
        {
            var p = GetById(id);
            if (p == null) return false;
            p.Stock += delta;
            return true;
        }

        public void UpdateProduct(Product p)
        {
            var existing = GetById(p.Id);
            if (existing == null) return;
            existing.Name = p.Name;
            existing.Price = p.Price;
            existing.Stock = p.Stock;
        }
    }
}
