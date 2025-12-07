using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Services.Implementations;

namespace Inlämningsuppgift_1.Repository.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserById(int id);
        User? GetUserByName(string token);
        IEnumerable<User> GetAllUsers();
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
