using Inlämningsuppgift_1.Services.Interfaces;

namespace Inlämningsuppgift_1.Services.Implementations
{
    public class ProductService : IProductService
    {
        private static readonly List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Pen", Price = 1.5m, Stock = 100 },
            new Product { Id = 2, Name = "Notebook", Price = 3.0m, Stock = 50 },
            new Product { Id = 3, Name = "Mug", Price = 6.0m, Stock = 20 }
        };

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public decimal Price { get; set; }

            // Jag tror inte att Product och ProductService ska hålla reda på lagersaldo, utan det bör finnas en egen service för lager.
            public int Stock { get; set; } 
        }

        public List<Product> GetAll() => Products;

        public Product? GetById(int id) => Products.FirstOrDefault(p => p.Id == id);

        public List<Product> Search(string? query)
        {
            if (string.IsNullOrWhiteSpace(query)) return Products.ToList();
            var q = query!.ToLowerInvariant();
           
            return Products.Where(p => p.Name.ToLowerInvariant().Contains(q) || p.Price.ToString().Contains(q)).ToList();
        }

        public Product Create(string name, decimal price, int stock)
        {
            var newId = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
            var p = new Product { Id = newId, Name = name, Price = price, Stock = stock };
            Products.Add(p);
            return p;
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
