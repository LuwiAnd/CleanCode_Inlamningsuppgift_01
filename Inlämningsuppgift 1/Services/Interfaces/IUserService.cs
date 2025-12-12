using Inlämningsuppgift_1.Entities;

namespace Inlämningsuppgift_1.Services.Interfaces
{
    public interface IUserService
    {
        bool Register(string username, string password, string email);
        string? Login(string username, string password);
        User? GetUserByToken(string token);
        User? GetUserById(int id);
    }
}
