using Inlämningsuppgift_1.Dto.Requests;
using Inlämningsuppgift_1.Dto.Responses;
using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Services.Interfaces
{
    public interface IUserService
    {
        public bool Register(UserRegisterRequest request);
        public UserLoginResponse? Login(UserLoginRequest request);
        public User? GetUserByToken(string token);
        public User? GetUserById(int id);

        public UserResponse ToUserResponse(User user);
        public UserLoginResponse ToUserLoginResponse(string token, User user);
    }
}
