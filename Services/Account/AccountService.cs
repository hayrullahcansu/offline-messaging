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

        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext dbContext
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
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

            //TODO: await _logService.Log(EventConstants.Login, null, user.Id, ip, LogEntry.Level.Info);

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
                //TODO: await _logService.Log(EventConstants.Register, null, user.Id, ip, LogEntry.Level.Info);
                serviceResult.Token = await GenerateJwtToken(user);

                return serviceResult;
            }

            //TODO: localization
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

        public async Task<List<UserActivity>> ListHistory(string userId)
        {
            return await _dbContext
                .UserActivities
                .Where(r => r.UserId == userId)
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