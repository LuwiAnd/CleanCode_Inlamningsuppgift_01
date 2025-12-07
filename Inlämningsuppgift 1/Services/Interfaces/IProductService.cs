using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Services.Interfaces
{
    public interface IProductService
    {
        Product? GetProductById(int id);
        IEnumerable<Product> GetAllProducts();
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
