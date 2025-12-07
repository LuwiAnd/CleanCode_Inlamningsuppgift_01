using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Services.Interfaces;

namespace Inlämningsuppgift_1.Services.Implementations
{
    public class CartService : ICartService
    {
        // Detta bör nog bytas ut mot en lista av Carts
        //private static readonly Dictionary<int, List<CartItem>> Carts = new Dictionary<int, List<CartItem>>();
        private static readonly List<Cart> Carts = new List<Cart>();

        

        public void AddToCart(int userId, int productId, int quantity)
        {
            //if (!Carts.ContainsKey(userId)) Carts[userId] = new List<CartItem>();
            //var list = Carts[userId];

            Cart? userCart = Carts
                .Where(c => c.UserId == userId)
                .FirstOrDefault();
            if (userCart == null) Carts.Add(new Cart { UserId = userId, CartItems = null });

            var productInCart = userCart!.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            if (productInCart == null) userCart!.CartItems?.Add(new CartItem { ProductId = productId, Quantity = quantity });
            else productInCart.Quantity += quantity;
        }

        //public IEnumerable<CartItem> GetCartForUser(int userId)
        public Cart? GetCartForUser(int userId)
        {
            //if (!Carts.ContainsKey(userId)) return Enumerable.Empty<CartItem>();
            Cart? userCart = Carts
                .Where(c => c.UserId == userId)
                .FirstOrDefault();

            return userCart;
        }

        public void RemoveFromCart(int userId, int productId)
        {
            //if (!Carts.ContainsKey(userId)) return;
            Cart? userCart = Carts
                .Where(c => c.UserId == userId)
                .FirstOrDefault();
            if (userCart == null) return;

            //var list = Carts[userId];
            //var existing = list.FirstOrDefault(ci => ci.ProductId == productId);
            //if (existing != null) list.Remove(existing);
            var productInCart = userCart!.CartItems?.RemoveAll(ci => ci.ProductId == productId);
        }

        public void ClearCart(int userId)
        {
            //if (Carts.ContainsKey(userId)) Carts[userId].Clear();
            Carts.RemoveAll(c => c.UserId == userId);
        }
    }
}
