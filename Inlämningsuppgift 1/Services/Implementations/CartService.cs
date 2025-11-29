namespace Inlämningsuppgift_1.Services.Implementations
{
    public class CartService
    {
      
        private static readonly Dictionary<int, List<CartItem>> Carts = new Dictionary<int, List<CartItem>>();

        public class CartItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public void AddToCart(int userId, int productId, int quantity)
        {
            if (!Carts.ContainsKey(userId)) Carts[userId] = new List<CartItem>();
            var list = Carts[userId];
            var existing = list.FirstOrDefault(ci => ci.ProductId == productId);
            if (existing == null) list.Add(new CartItem { ProductId = productId, Quantity = quantity });
            else existing.Quantity += quantity;
        }

        public IEnumerable<CartItem> GetCartForUser(int userId)
        {
            if (!Carts.ContainsKey(userId)) return Enumerable.Empty<CartItem>();
            return Carts[userId];
        }

        public void RemoveFromCart(int userId, int productId)
        {
            if (!Carts.ContainsKey(userId)) return;
            var list = Carts[userId];
            var existing = list.FirstOrDefault(ci => ci.ProductId == productId);
            if (existing != null) list.Remove(existing);
        }

        public void ClearCart(int userId)
        {
            if (Carts.ContainsKey(userId)) Carts[userId].Clear();
        }
    }
}
