﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Attributes;
using OfflineMessaging.Entities;
using OfflineMessaging.Exceptions;
using OfflineMessaging.Extensions;
using OfflineMessaging.Services.Account;
using OfflineMessaging.Services.Messaging;

namespace OfflineMessaging.Controllers
{
    [Route("api/v1/[controller]")]
    public class MessageController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly IMessageService _messageService;
        private readonly ApplicationDbContext _dbContext;

        public MessageController(
            ApplicationDbContext dbContext,
            IAccountService accountService,
            IMessageService messageService,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _accountService = accountService;
            _userManager = userManager;
            _messageService = messageService;
        }

        // POST api/v1/message
        [JwtAuthorize]
        [HttpPost]
        public SendMessageServiceResult Post([FromBody] MessageInput input)
        {
            var targetUser = _dbContext.Users.FirstOrDefaultAsync(r => r.UserName.Equals(input.Username)).GetAwaiter()
                .GetResult();
            if (targetUser == null) throw new NotFoundException("User Not Found");
            if (input.Text.Length > 5000) throw new PayloadLargeException("Text Message Too Large");

            var userId = User.GetUserId();
            var userBlock = _dbContext.UserBlocks
                .FirstOrDefaultAsync(r => r.BlockedId == userId && r.BlockerId == targetUser.Id).GetAwaiter()
                .GetResult();
            if (userBlock != null)
                return new SendMessageServiceResult().AddError(String.Empty, "Blocked to send message by user") as
                    SendMessageServiceResult;

            return _messageService.SendMessage(userId, targetUser.Id, input.Text).GetAwaiter().GetResult();
        }

        // GET api/v1/message/{targetUserId}/{fromIndex}/{limit}
        [JwtAuthorize]
        [HttpGet("{targetUserId}/{fromIndex}/{limit}")]
        public GetMessageServiceResult Get(string targetUserId, int fromIndex = 0, int limit = 20)
        {
            var targetUser = _userManager.FindByIdAsync(targetUserId).GetAwaiter().GetResult();
            if (targetUser == null) throw new NotFoundException("User Not Found");
            var userId = User.GetUserId();
            return _messageService.GetMessage(userId, targetUser.Id, fromIndex, limit).GetAwaiter().GetResult();
        }
    }
}