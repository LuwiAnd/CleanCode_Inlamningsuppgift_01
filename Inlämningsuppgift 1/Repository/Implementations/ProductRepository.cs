using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;

namespace Inlämningsuppgift_1.Repository.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private static readonly List<Product> _products = new List<Product>();

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

            _products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            var existing = GetProductById(product.Id);
            if (existing != null)
                _products.Remove(existing);

            _products.Add(product);
        }

        public void DeleteProduct(int id)
        {
            _products.RemoveAll(p => p.Id == id);
        }

    }
}
