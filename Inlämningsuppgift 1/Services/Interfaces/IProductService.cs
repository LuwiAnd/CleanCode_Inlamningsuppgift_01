using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Services.Interfaces
{
    public interface IProductService
    {
        public List<Product> GetAllProducts();
        public Product? GetProductById(int id);
        public List<Product> Search(string? query);
        //void CreateProduct(Product product);
        public void CreateProduct(string name, decimal price, int stockBalance);
        public void UpdateProduct(Product product);
        public void DeleteProduct(int id);
        public bool ChangeProductStock(int productId, int delta);
    }
}
