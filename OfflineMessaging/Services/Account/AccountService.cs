using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfflineMessaging.Dtos;
using OfflineMessaging.Entities;
using OfflineMessaging.Exceptions;
using OfflineMessaging.Services.Logging;
using OfflineMessaging.Utils;


namespace OfflineMessaging.Services.Account
{
    public class LoginServiceResult : ServiceResult
    {
        public object Token { get; set; }
    }

    public class RegisterServiceResult : ServiceResult
    {
        public object Token { get; set; }
    }


    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILoggerService _loggerService;

        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext dbContext,
            ILoggerService loggerService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _loggerService = loggerService;
        }

        public async Task<LoginServiceResult> Login(LoginInput input, string ip)
        {
            var serviceResult = new LoginServiceResult();
            var user = await _dbContext.Users.FirstOrDefaultAsync(r => r.UserName.Equals(input.UserName));

            if (user == null)
            {
                serviceResult.SetFailed(9128);

                return serviceResult;
            }

            var passwordMatch = await _userManager.CheckPasswordAsync(user, input.Password);
            if (passwordMatch)
            {
                serviceResult.Token = await GenerateJwtToken(user);
            }
            else
            {
                serviceResult.SetFailed(1001);
            }

            await _loggerService.AddUserActivity(user.Id, EventConstants.Login, $"User logined at {ip}");

            return serviceResult;
        }

        public async Task<RegisterServiceResult> Register(RegisterInput registerInput, string ip)
        {
            var user = new User
            {
                UserName = registerInput.Username,
                Email = registerInput.Email,
                Name = registerInput.Name,
                Surname = registerInput.Surname,
                RegisterDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registerInput.Password);
            var serviceResult = new RegisterServiceResult();

            if (result.Succeeded)
            {
                await _dbContext.SaveChangesAsync();
                await _signInManager.SignInAsync(user, isPersistent: true);
                await _loggerService.AddUserActivity(user.Id, EventConstants.Register, $"User registered at {ip}");
                serviceResult.Token = await GenerateJwtToken(user);

                return serviceResult;
            }

            foreach (var error in result.Errors)
            {
                serviceResult.AddError(error.Code, error.Description);
            }

            return serviceResult;
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<UserDto> GetUserByUsername(string userName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(r => r.UserName.Equals(userName));
            if (user == null)
                return null;
            return new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                DateOfBirth = user.DateOfBirth,
                RegisterDate = user.RegisterDate,
                LastLoginDate = user.LastLoginDate,
                UserName = user.UserName
            };
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<List<UserActivity>> ListHistory(string userName)
        {
            var user = GetUserByUsername(userName).GetAwaiter().GetResult();
            if (user == null) throw new NotFoundException("User Not Found");
            return await _dbContext
                .UserActivities
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.Id)
                .Take(20)
                .ToListAsync();
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Surname, user.Surname ?? ""),
                new Claim(ClaimTypes.Role, roles.SingleOrDefault() ?? ""),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(Constants.JwtExpireDays));

            var token = new JwtSecurityToken(
                Constants.JwtIssuer,
                Constants.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}