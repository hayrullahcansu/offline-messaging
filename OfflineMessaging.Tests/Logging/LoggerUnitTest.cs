using OfflineMessaging.Entities;
using OfflineMessaging.Services.Logging;
using OfflineMessaging.Services.Messaging;
using Xunit;

namespace OfflineMessaging.Tests.Logging
{
    public class LoggerUnitTest
    {
        private readonly ILoggerService _loggerService;
        private readonly ApplicationDbContext _dbContext;

        public LoggerUnitTest()
        {
            _dbContext = new ApplicationDbContext();

            //IAccountService accountService, UserManager<User> userManager
            //  AccountService service = new AccountService();
        }

        ~LoggerUnitTest()
        {
            _dbContext.Dispose();
        }

        [Fact]
        public void AddCustomLog()
        {
            var service = new LoggerService(_dbContext);
            var result = service.AddLogEntry("unit_test","test",0,"hayro",false).GetAwaiter().GetResult();
            Assert.True(result.Succeed, "Custom Log Adding Failed");
        }

        [Fact]
        public void AddUndefinedUserActivity()
        {
            var service = new LoggerService(_dbContext);
            var result = service.AddUserActivity("undefined_user_id","test","test").GetAwaiter().GetResult();
            Assert.True(result.Succeed, "Adding Undefined User Activity Failed");
        }
    }
}