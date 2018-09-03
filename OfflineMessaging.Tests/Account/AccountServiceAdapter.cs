using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OfflineMessaging.Dtos;
using OfflineMessaging.Entities;
using OfflineMessaging.Services.Account;

namespace OfflineMessaging.Tests.Account
{
    public class AccountServiceAdapter : IAccountService
    {
        public Task<LoginServiceResult> Login(LoginInput input, string ip)
        {
            return new Task<LoginServiceResult>(() => new LoginServiceResult());
        }

        public Task<RegisterServiceResult> Register(RegisterInput registerInput, string ip)
        {
            return new Task<RegisterServiceResult>((() => new RegisterServiceResult()));
        }

        public Task<User> GetUserById(string userId)
        {
            return new Task<User>((() => new User()));
        }

        public Task<UserDto> GetUserByUsername(string userName)
        {
            return new Task<UserDto>((() => new UserDto()));
        }

        public Task<User> GetUserByEmail(string email)
        {
            return new Task<User>((() => new User()));
        }

        public Task<List<UserActivity>> ListHistory(string userName)
        {
            return new Task<List<UserActivity>>((() => new List<UserActivity>()));
        }
    }
}