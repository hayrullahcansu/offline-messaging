using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfflineMessaging.Attributes;
using OfflineMessaging.Dtos;
using OfflineMessaging.Entities;
using OfflineMessaging.Exceptions;
using OfflineMessaging.Services.Account;

namespace OfflineMessaging.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;

        public AccountController(IAccountService accountService, UserManager<User> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        // POST api/v1/account
        [HttpPost]
        public RegisterServiceResult Post([FromBody] RegisterInput input)
        {
            return _accountService.Register(input, HttpContext.Connection.RemoteIpAddress.ToString())
                .GetAwaiter()
                .GetResult();
        }

        // POST api/v1/account/login
        [HttpPost("login")]
        public LoginServiceResult Login([FromBody] LoginInput input)
        {
            return _accountService.Login(input, HttpContext.Connection.RemoteIpAddress.ToString())
                .GetAwaiter()
                .GetResult();
        }


        // GET api/v1/account/{userName}
        [HttpGet("{userName}")]
        public UserDto Get(string userName)
        {
            var dto = _accountService.GetUserByUsername(userName).GetAwaiter().GetResult();
            if (dto == null) throw new NotFoundException("User Not Found");
            return dto;
        }

        // GET api/v1/account/{id}/history
        [JwtAuthorize]
        [HttpGet("{userName}/history")]
        public List<UserActivity> GetHistory(string userName)
        {
            return _accountService.ListHistory(userName)
                .GetAwaiter()
                .GetResult();
        }
    }
}