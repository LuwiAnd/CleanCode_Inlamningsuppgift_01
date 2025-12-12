using Inlämningsuppgift_1.Services.Interfaces;
using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Dto.Requests;
using Inlämningsuppgift_1.Repository.Interfaces;


namespace Inlämningsuppgift_1.Services.Implementations
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;

  
        private static readonly Dictionary<string, int> Tokens = new Dictionary<string, int>();

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        

        public bool Register(string username, string password, string email)
        {
            CreateUserRequest? request = new CreateUserRequest { 
                Username = username, 
                Password = password, 
                Email = email 
            };

            var userWasCreated = _repository.CreateUser(request);

            return userWasCreated;
        }

        public string? Login(string username, string password)
        {
            var user = _repository.GetUserByName(username);
            if (user == null || user.Password != password)
                return null;

            var token = "token-" + Guid.NewGuid();
            Tokens[token] = user.Id;

            return token;
        }

        public User? GetUserByToken(string token)
        {
            if (Tokens.TryGetValue(token, out var userId))
            {
                return _repository.GetUserById(userId);
            }

            return null;
        }

        public User? GetUserById(int id) => _repository.GetUserById(id);
    }
}
