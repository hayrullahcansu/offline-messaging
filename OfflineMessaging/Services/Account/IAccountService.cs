
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using OfflineMessaging.Dtos;
using OfflineMessaging.Entities;

namespace OfflineMessaging.Services.Account
{
    public interface IAccountService
    {
        Task<LoginServiceResult> Login(LoginInput input, string ip);
        Task<RegisterServiceResult> Register(RegisterInput registerInput, string ip);
        Task<User> GetUserById(string userId);
        Task<UserDto> GetUserByUsername(string userName);
        Task<User> GetUserByEmail(string email);
        Task<List<UserActivity>> ListHistory(string userId);
    }
}