using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Entities;
using OfflineMessaging.Services.Account;

namespace OfflineMessaging.Services.Messaging
{
    public class SendMessageServiceResult : ServiceResult
    {
        public Message Message { get; set; }
    }

    public class GetMessageServiceResult : ServiceResult
    {
        public List<Message> Messages { get; set; }
    }

    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageService(
            ApplicationDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }


        public async Task<SendMessageServiceResult> SendMessage(
            string senderUserId,
            string recieverUserId,
            string text)
        {
            var result = new SendMessageServiceResult();

            try
            {
                Message message = new Message()
                {
                    SenderId = senderUserId,
                    RecieverId = recieverUserId,
                    Text = text
                };
                _dbContext.Messages.Add(message);
                await _dbContext.SaveChangesAsync();
                result.Message = message;
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
                //TODO: Log
            }

            return result;
        }

        public async Task<GetMessageServiceResult> GetMessage(
            string userId,
            string targetUserId,
            int fromIndex = 0,
            int limit = 20)
        {
            var result = new GetMessageServiceResult();

            try
            {
                var list = await _dbContext.Messages.Where(r =>
                    (
                        (r.RecieverId == userId && r.SenderId == targetUserId) ||
                        (r.RecieverId == targetUserId && r.SenderId == userId)
                    ) &&
                    (fromIndex <= 0 || r.Id < fromIndex)
                ).OrderByDescending(r => r.Id).Take(limit).ToListAsync();
                result.Messages = list;
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
                //TODO: Log
            }

            return result;
        }
    }
}