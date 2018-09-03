using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfflineMessaging.Entities;
using OfflineMessaging.Services.Account;
using OfflineMessaging.Services.Logging;

namespace OfflineMessaging.Controllers
{
    [Route("api/v1/[controller]")]
    public class LoggingController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILoggerService _loggerService;
        private readonly UserManager<User> _userManager;

        public LoggingController(
            IAccountService accountService,
            UserManager<User> userManager,
            ILoggerService loggerService
        )
        {
            _accountService = accountService;
            _userManager = userManager;
            _loggerService = loggerService;
        }

        // GET api/v1/logging/{fromIndex}{limit}
        [HttpGet("{fromIndex}{limit}")]
        public GetLogEntriesServiceResult Get(int fromIndex, int limit)
        {
            return _loggerService.GetLogEntries(fromIndex, limit).GetAwaiter().GetResult();
        }
    }
}