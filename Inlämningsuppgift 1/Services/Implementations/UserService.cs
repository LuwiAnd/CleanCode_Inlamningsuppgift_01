using Inlämningsuppgift_1.Services.Interfaces;

namespace Inlämningsuppgift_1.Services.Implementations
{
    public class UserService : IUserService
    {
       
        private static readonly List<User> Users = new List<User>
        {
            new User { Id = 1, Username = "alice", Password = "password", Email = "alice@example.com" },
            new User { Id = 2, Username = "bob", Password = "password", Email = "bob@example.com" }
        };

  
        private static readonly Dictionary<string, int> Tokens = new Dictionary<string, int>();

        

        public bool Register(string username, string password, string email)
        {
            if (Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) return false;
            var id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
            Users.Add(new User { Id = id, Username = username, Password = password, Email = email });
            return true;
        }

        public string? Login(string username, string password)
        {
            var u = Users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && x.Password == password);
            if (u == null) return null;
            var token = "token-" + Guid.NewGuid().ToString(); 
            Tokens[token] = u.Id;
            return token;
        }

        public User? GetUserByToken(string token)
        {
            if (Tokens.TryGetValue(token, out var userId))
            {
                return Users.FirstOrDefault(u => u.Id == userId);
            }
            return null;
        }

        public User? GetById(int id) => Users.FirstOrDefault(u => u.Id == id);
    }
}
