using Inlämningsuppgift_1.Dto.Responses;
using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Repository.Interfaces
{
    public interface IProductRepository
    {
        public Product? GetProductById(int id);
        public IEnumerable<Product> GetAll();
        public void CreateProduct(Product product);
        public void UpdateProduct(Product product);
        public void DeleteProduct(int id);

    }
}

