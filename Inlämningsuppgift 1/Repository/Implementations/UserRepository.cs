using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;

namespace Inlämningsuppgift_1.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new List<User>();

        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByName(string token)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users.ToList();
        }

        public void CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _users.Add(user);
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
