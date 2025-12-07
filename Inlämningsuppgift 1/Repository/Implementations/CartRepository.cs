using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;
using System.Threading.Tasks;

namespace Inlämningsuppgift_1.Repository.Implementations
{
    public class CartRepository : ICartRepository
    {
        private static readonly List<Cart> _carts = new List<Cart>();

        public void CreateCart(Cart cart)
        {
            if (cart == null || cart.UserId <= 0)
                throw new ArgumentException("Cart must have a valid UserId before being saved.");

            if (cart.CartItems == null)
                cart.CartItems = new List<CartItem>();

            _carts.Add(cart);
        }

        public void DeleteCart(int userId)
        {
            _carts.RemoveAll(c => c.UserId == userId);
        }

        public List<Cart> GetAllCarts()
        {
            // Jag använder ToList() för att inte returnera originallistan, så 
            // att ingen kan modifiera den utanför detta program.
            return _carts.ToList();
        }

        public Cart? GetCartByUserId(int userId)
        {
            return _carts.FirstOrDefault(c => c.UserId == userId);
        }

        public void UpdateCart(Cart cart)
        {
            var existingCart = GetCartByUserId(cart.UserId);
            if (existingCart != null)
                _carts.Remove(existingCart);

            _carts.Add(cart);
        }
    }
}
