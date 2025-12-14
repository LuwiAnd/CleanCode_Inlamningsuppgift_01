using Inlämningsuppgift_1.Dto.Requests;
using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;

namespace Inlämningsuppgift_1.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "alice", Password = "password", Email = "alice@example.com" },
            new User { Id = 2, Username = "bob", Password = "password", Email = "bob@example.com" }
        };
        
        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        // Jag hanterar tokens i UserService. Repositoryn ska inte behöva känna till
        // tokens. UserService ska därför ta fram username från token och skicka hit.
        public User? GetUserByName(string username)
        {
            return _users.FirstOrDefault(u =>
                u.Username.Equals(
                    username, 
                    StringComparison.OrdinalIgnoreCase
                )
            );
        }

        


        public IEnumerable<User> GetAllUsers()
        {
            return _users.ToList();
        }

        public bool CreateUser(UserRegisterRequest userRequest)
        {
            // Varför inte behålla denna?
            if (userRequest == null)
                throw new ArgumentNullException(nameof(userRequest));

            if (_users.Any(u => u.Username == userRequest.Username))
                return false;
            
            var id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;

            var user = new User { 
                Id = id, 
                Username = userRequest.Username, 
                Password = userRequest.Password, 
                Email = userRequest.Email 
            };

            _users.Add(user);

            return true;
        }

        public void UpdateUser(User user)
        {
            var existingUser = GetUserById(user.Id);
            if (existingUser != null)
                _users.Remove(existingUser);

            _users.Add(user);
        }

        public void DeleteUser(int id)
        {
            _users.RemoveAll(u => u.Id == id);
        }

    }
}
