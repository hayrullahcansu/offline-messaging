using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Entities;
using OfflineMessaging.Services.Logging;
using OfflineMessaging.Utils;

namespace OfflineMessaging.Services.UserManagement
{
    public class BlockUserServiceResult : ServiceResult
    {
        public UserBlock UserBlock { get; set; }
    }

    public class UnblockUserServiceResult : ServiceResult
    {
        public UserBlock UnblockedUser { get; set; }
    }

    public class GetBlockListServiceResult : ServiceResult
    {
        public List<UserBlock> UserBlockList { get; set; }
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILoggerService _loggerService;

        public UserManagementService(
            ApplicationDbContext dbContext,
            ILoggerService loggerService
        )
        {
            _dbContext = dbContext;
            _loggerService = loggerService;
        }

        public async Task<BlockUserServiceResult> BlockUser(string sourceUserId, string targetUserId)
        {
            var result = new BlockUserServiceResult();

            try
            {
                UserBlock userBlock = new UserBlock()
                {
                    BlockerId = sourceUserId,
                    BlockedId = targetUserId
                };

                _dbContext.UserBlocks.Add(userBlock);
                await _dbContext.SaveChangesAsync();
                result.UserBlock = userBlock;
                await _loggerService.AddUserActivity(sourceUserId, EventConstants.BlockUser, $"User blocked user {targetUserId}");
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
                await _loggerService.AddLogEntry(ex.GetBaseException().ToString(), ex.ToString());
            }

            return result;
        }

        public async Task<UnblockUserServiceResult> UnblockUser(string sourceUserId, string targetUserId)
        {
            var result = new UnblockUserServiceResult();

            try
            {
                var userBlock = _dbContext.UserBlocks.FirstOrDefault(r =>
                    r.BlockerId == sourceUserId &&
                    r.BlockedId == targetUserId
                );
                if (userBlock == null)
                {
                    result.SetFailed("1005");
                    return result;
                }

                _dbContext.UserBlocks.Remove(userBlock);
                await _dbContext.SaveChangesAsync();
                result.UnblockedUser = userBlock;
                await _loggerService.AddUserActivity(sourceUserId, EventConstants.UnblockUser, $"User unblocked user {targetUserId}");
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
                await _loggerService.AddLogEntry(ex.GetBaseException().ToString(), ex.ToString());
            }

            return result;
        }

        public async Task<GetBlockListServiceResult> GetBlockList(string sourceUserId)
        {
            var result = new GetBlockListServiceResult();
            try
            {
                var list = await _dbContext.UserBlocks.Where(r =>
                    r.BlockerId == sourceUserId
                ).ToListAsync();
                result.UserBlockList = list;
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
                await _loggerService.AddLogEntry(ex.GetBaseException().ToString(), ex.ToString());
            }

            return result;
        }
    }
}