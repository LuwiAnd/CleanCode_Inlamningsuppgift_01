using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Repository.Interfaces
{
    
    public interface ICartRepository
    {
        Cart? GetCartByUserId(int userId);
        void CreateCart(Cart cart);
        void UpdateCart(Cart cart);
        void DeleteCart(int userId);
        List<Cart> GetAllCarts();
    }
}
