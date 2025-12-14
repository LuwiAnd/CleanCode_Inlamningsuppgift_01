using Inlämningsuppgift_1.Dto.Requests;
using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Services.Implementations;

namespace Inlämningsuppgift_1.Repository.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserById(int id);
        User? GetUserByName(string username);
        IEnumerable<User> GetAllUsers();
        bool CreateUser(UserRegisterRequest userRequest);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
