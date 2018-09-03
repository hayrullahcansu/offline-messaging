using System.Collections.Generic;
using System.Threading.Tasks;
using OfflineMessaging.Entities;
using OfflineMessaging.Services.Messaging;

namespace OfflineMessaging.Services.UserManagement
{
    public interface IUserManagementService
    {
        Task<BlockUserServiceResult> BlockUser(string sourceUserId, string targetUserId);
        Task<UnblockUserServiceResult> UnblockUser(string sourceUserId, string targetUserId);
        Task<GetBlockListServiceResult> GetBlockList(string sourceUserId);
    }
}