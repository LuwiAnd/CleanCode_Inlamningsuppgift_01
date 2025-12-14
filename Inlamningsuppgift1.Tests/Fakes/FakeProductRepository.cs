using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;

namespace Inlamningsuppgift1.Tests.Fakes
{
    public class FakeProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

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
            // Fake auto-increment
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
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
