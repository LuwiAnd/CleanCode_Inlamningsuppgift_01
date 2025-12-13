using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Repository.Interfaces
{
    
    public interface ICartRepository
    {
        public Cart? GetCartByUserId(int userId);
        public void CreateCart(Cart cart);
        public void UpdateCart(Cart cart);
        public void DeleteCart(int userId);
        public List<Cart> GetAllCarts();
    }
}
