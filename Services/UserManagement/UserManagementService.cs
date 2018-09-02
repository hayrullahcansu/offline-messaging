using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Entities;

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

        public UserManagementService(
            ApplicationDbContext dbContext
        )
        {
            _dbContext = dbContext;
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
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
                //TODO: Log
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
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
                //TODO: Log
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
                //TODO: Log
            }

            return result;
        }
    }
}