using System.Collections.Generic;
using System.Threading.Tasks;
using OfflineMessaging.Entities;
using OfflineMessaging.Services.Account;

namespace OfflineMessaging.Services.Messaging
{
    public interface IMessageService
    {
        Task<SendMessageServiceResult> SendMessage(string senderUserId, string recieverUserId, string text);
        Task<GetMessageServiceResult> GetMessage(string userId, string targetUserId, int fromIndex = 0, int limit = 20);
    }
}