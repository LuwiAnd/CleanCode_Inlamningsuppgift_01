using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Inlamningsuppgift1.Tests.Fakes
{
    public class FakeCartRepository : ICartRepository
    {
        private readonly List<Cart> _carts = new();

        public Cart? GetCartByUserId(int userId)
        {
            return _carts.FirstOrDefault(c => c.UserId == userId);
        }

        public void CreateCart(Cart cart)
        {
            // Ingen validering – i tester vill vi bara spara data
            if (cart.CartItems == null)
                cart.CartItems = new List<CartItem>();

            _carts.Add(cart);
        }

        public void UpdateCart(Cart cart)
        {
            var existing = GetCartByUserId(cart.UserId);
            if (existing != null)
                _carts.Remove(existing);

            _carts.Add(cart);
        }

        public void DeleteCart(int userId)
        {
            _carts.RemoveAll(c => c.UserId == userId);
        }

        public List<Cart> GetAllCarts()
        {
            // Returnera kopia, precis som riktiga repo gör
            return _carts.ToList();
        }
    }
}

