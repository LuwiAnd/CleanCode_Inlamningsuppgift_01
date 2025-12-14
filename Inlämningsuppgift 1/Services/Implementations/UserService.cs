using Inlämningsuppgift_1.Dto.Requests;
using Inlämningsuppgift_1.Dto.Responses;
using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Repository.Interfaces;
using Inlämningsuppgift_1.Services.Interfaces;


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


        public bool Register(UserRegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
                return false;

            return _repository.CreateUser(request);
        }

        public UserLoginResponse? Login(UserLoginRequest request)
        {
            var user = _repository.GetUserByName(request.Username);

            if (user == null || user.Password != request.Password)
                return null;

            var token = "token-" + Guid.NewGuid();
            Tokens[token] = user.Id;

            return ToUserLoginResponse(token, user);
        }

        public User? GetUserByToken(string token)
        {
            if (Tokens.TryGetValue(token, out var userId))
            {
                return _repository.GetUserById(userId);
            }

            return null;
        }

        



        public User? GetUserById(int id) => 
            _repository.GetUserById(id);


        public UserResponse ToUserResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public UserLoginResponse ToUserLoginResponse(string token, User user)
        {
            return new UserLoginResponse
            {
                Token = token,
                UserId = user.Id
            };
        }

    }
}
