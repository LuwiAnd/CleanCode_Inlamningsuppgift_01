using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;
using Inlämningsuppgift_1.Services.Interfaces;

namespace Inlämningsuppgift_1.Services.Implementations
{
    public class CartService : ICartService
    {
        // Detta bör nog bytas ut mot en lista av Carts
        //private static readonly Dictionary<int, List<CartItem>> Carts = new Dictionary<int, List<CartItem>>();
        //private static readonly List<Cart> Carts = new List<Cart>();
        // Jag har flyttat ovanstående till CartRepository.

        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        //public IEnumerable<CartItem> GetCartForUser(int userId)
        public Cart? GetCartForUser(int userId)
        {
            return _repository.GetCartByUserId(userId);
        }

        public void AddToCart(int userId, int productId, int quantity)
        {
            var cart = _repository.GetCartByUserId(userId);

            if(cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                _repository.CreateCart(cart);
            }

            if (cart.CartItems == null)
                cart.CartItems = new List<CartItem>();

            var item = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (item == null)
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }

            _repository.UpdateCart(cart);


        }



        public void RemoveFromCart(int userId, int productId)
        {
            var cart = _repository.GetCartByUserId(userId);
            if (cart == null || cart.CartItems == null)
                return;

            cart.CartItems.RemoveAll(ci => ci.ProductId == productId);

            _repository.UpdateCart(cart);
        }

        public void ClearCart(int userId)
        {
            _repository.DeleteCart(userId);
        }
    }
}
