using Inlämningsuppgift_1.Services.Implementations;
using Inlämningsuppgift_1.Dto.Requests;
using Inlamningsuppgift1.Tests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Inlamningsuppgift1.Tests.Tests.UserTests
{
    public class UserServiceTests
    {
        // --------------------------------------------------------------------
        // 1. Login returnerar token vid korrekt lösenord
        // --------------------------------------------------------------------
        [Fact]
        public void Login_ShouldReturnToken_WhenCredentialsAreCorrect()
        {
            // ARRANGE
            var repo = new FakeUserRepository();
            var service = new UserService(repo);

            // skapa användare
            repo.CreateUser(new UserRegisterRequest
            {
                Username = "alice",
                Password = "password",
                Email = "alice@example.com"
            });

            var request = new UserLoginRequest
            {
                Username = "alice",
                Password = "password"
            };

            // ACT
            var result = service.Login(request);

            // ASSERT
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result!.Token));
            Assert.Equal(1, result.UserId);
        }


        // --------------------------------------------------------------------
        // 2. Login returnerar null vid fel lösenord
        // --------------------------------------------------------------------
        [Fact]
        public void Login_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // ARRANGE
            var repo = new FakeUserRepository();
            var service = new UserService(repo);

            repo.CreateUser(new UserRegisterRequest
            {
                Username = "bob",
                Password = "password",
                Email = "bob@example.com"
            });

            var request = new UserLoginRequest
            {
                Username = "bob",
                Password = "wrong"
            };

            // ACT
            var result = service.Login(request);

            // ASSERT
            Assert.Null(result);
        }

        // --------------------------------------------------------------------
        // 3. Register returnerar false när användarnamnet redan finns
        // --------------------------------------------------------------------
        [Fact]
        public void Register_ShouldReturnFalse_WhenUsernameAlreadyExists()
        {
            // ARRANGE
            var repo = new FakeUserRepository();
            var service = new UserService(repo);

            var req = new UserRegisterRequest
            {
                Username = "alice",
                Password = "123",
                Email = "alice@example.com"
            };
            service.Register(req);

            var duplicateUser = new UserRegisterRequest
            {
                Username = req.Username, // test att felet fail:ar med annat username + "a",
                Password = "abc",
                Email = "other.email@example.com"
            };

            // ACT – försök registrera med samma username
            var result = service.Register(duplicateUser);

            // ASSERT
            Assert.False(result);
        }


        // --------------------------------------------------------------------
        // 4. GetUserByToken returnerar rätt användare
        // --------------------------------------------------------------------
        [Fact]
        public void GetUserByToken_ShouldReturnCorrectUser()
        {
            // ARRANGE
            var repo = new FakeUserRepository();
            var service = new UserService(repo);

            repo.CreateUser(new UserRegisterRequest
            {
                Username = "bob",
                Password = "pw",
                Email = "bob@example.com"
            });

            var loginResponse = service.Login(new UserLoginRequest
            {
                Username = "bob",
                Password = "pw"
            });

            Assert.NotNull(loginResponse);

            // ACT – hämta user via token
            var user = service.GetUserByToken(loginResponse!.Token);

            // ASSERT
            Assert.NotNull(user);
            Assert.Equal("bob", user!.Username);
        }


        // --------------------------------------------------------------------
        // 5. GetUserByToken returnerar null vid okänt token
        // --------------------------------------------------------------------
        [Fact]
        public void GetUserByToken_ShouldReturnNull_WhenTokenDoesNotExist()
        {
            // ARRANGE
            var repo = new FakeUserRepository();
            var service = new UserService(repo);

            // ACT
            var user = service.GetUserByToken("wrong-token-xyz");

            // ASSERT
            Assert.Null(user);
        }
    }
}
