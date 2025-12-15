using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;
using Inlämningsuppgift_1.Dto.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Inlamningsuppgift1.Tests.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByName(string username)
        {
            return _users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users.ToList();
        }

        public bool CreateUser(UserRegisterRequest request)
        {
            if (_users.Any(u => u.Username == request.Username))
                return false;

            var id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;

            _users.Add(new User
            {
                Id = id,
                Username = request.Username,
                Password = request.Password,
                Email = request.Email
            });

            return true;
        }

        public void UpdateUser(User user)
        {
            var existing = GetUserById(user.Id);
            if (existing != null)
                _users.Remove(existing);

            _users.Add(user);
        }

        public void DeleteUser(int id)
        {
            _users.RemoveAll(u => u.Id == id);
        }
    }
}
