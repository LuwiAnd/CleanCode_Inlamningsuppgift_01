using Inlämningsuppgift_1.Dto.Responses;
using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Services.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<Product> GetAllProducts();
        public Product? GetProductById(int id);
        public IEnumerable<Product> Search(string? query);
        public Product CreateProduct(string name, decimal price, int stockBalance);
        public void UpdateProduct(Product product);
        public void DeleteProduct(int id);
        public bool ChangeProductStock(int productId, int delta);

        public ProductResponse ToProductResponse(Product p);

        public IEnumerable<ProductResponse> ToProductResponseList(IEnumerable<Product> products);

    }
}
