using Inlämningsuppgift_1.Dto.Responses;
using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;
using Inlämningsuppgift_1.Services.Interfaces;

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

        public IEnumerable<Product> Search(string? query)
        {
            if (string.IsNullOrWhiteSpace(query)) 
                return GetAllProducts();

            var q = query!.ToLowerInvariant();

            var products = GetAllProducts();

            return products
                .Where(
                    p =>
                    p.Name.ToLowerInvariant().Contains(q)
                    ||
                    p.Price.ToString().Contains(q)
                );
        }

        public Product CreateProduct(
            string name, 
            decimal price, 
            int stockBalance
        )
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
            //var newId = products.Any() ? products.Max(p => p.Id) + 1 : 1; // Detta ska jag flytta till repo sen!!
                                                                          // ??!!???
            var product = new Product 
            { 
                //Id = newId, 
                Name = name, 
                Price = price, 
                StockBalance = stockBalance 
            };

            _repository.CreateProduct(product);

            return product;
        }

        public bool ChangeProductStock(int productId, int delta)
        {
            var product = _repository.GetProductById(productId);

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



        public ProductResponse ToProductResponse(Product p)
        {
            return new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockBalance = p.StockBalance
            };
        }

        public IEnumerable<ProductResponse> ToProductResponseList(IEnumerable<Product> products)
        {
            return products.Select(ToProductResponse);
        }


    }
}
