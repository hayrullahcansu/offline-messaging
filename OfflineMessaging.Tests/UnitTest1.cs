using System;
using Microsoft.AspNetCore.Identity;
using OfflineMessaging.Controllers;
using OfflineMessaging.Entities;
using OfflineMessaging.Services.Account;
using OfflineMessaging.Services.Messaging;
using Xunit;

namespace OfflineMessaging.Tests
{
    public class UnitTest1
    {
        private readonly IMessageService _messageService;

        public UnitTest1()
        {
            //IAccountService accountService, UserManager<User> userManager
            //  AccountService service = new AccountService();

            
        }

        [Fact]
        public void Test1()
        {
            
            
            Assert.True(true,"Passed");
        }
    }
}