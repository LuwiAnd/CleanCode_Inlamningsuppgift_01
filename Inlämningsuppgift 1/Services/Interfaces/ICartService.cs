using Inlämningsuppgift_1.Dto.Responses;
using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Services.Interfaces
{
    public interface ICartService
    {
        void AddToCart(int userId, int productId, int quantity);
        void RemoveFromCart(int userId, int productId);
        void ClearCart(int userId);
        Cart? GetCartForUser(int userId);
        public CartResponse ToCartResponse(Cart cart);
    }
}
