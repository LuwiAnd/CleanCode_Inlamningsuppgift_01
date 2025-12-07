using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Repository.Interfaces
{
    public interface IProductRepository
    {
        Product? GetProductById(int id);
        IEnumerable<Product> GetAll();
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}

