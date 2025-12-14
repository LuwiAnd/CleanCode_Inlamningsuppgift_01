using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Inlamningsuppgift1.Tests.Fakes
{
    public class FakeOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();
        private int _nextId = 1;

        public Order CreateOrder(Order order)
        {
            order.Id = _nextId++;
            _orders.Add(order);
            return order;
        }

        public Order? GetOrderById(int orderId)
        {
            return _orders.FirstOrDefault(o => o.Id == orderId);
        }

        public IEnumerable<Order> GetOrdersForUser(int userId)
        {
            return _orders.Where(o => o.UserId == userId);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orders.ToList();
        }

        public void UpdateOrder(Order order)
        {
            var existing = GetOrderById(order.Id);
            if (existing != null)
                _orders.Remove(existing);

            _orders.Add(order);
        }

        public void DeleteOrder(int orderId)
        {
            _orders.RemoveAll(o => o.Id == orderId);
        }
    }
}
