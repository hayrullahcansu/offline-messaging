using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Entities;

namespace OfflineMessaging.Services.Logging
{
    public class GetLogEntriesServiceResult : ServiceResult
    {
        public List<LogEntry> LogEntries { get; set; }
    }

    public class LoggerService : ILoggerService
    {
        private readonly ApplicationDbContext _dbContext;

        public LoggerService(
            ApplicationDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> AddUserActivity(string userId, string operation, string text)
        {
            var result = new ServiceResult();
            var user = _dbContext.Users.FirstOrDefaultAsync(r => r.Id == userId)
                .GetAwaiter()
                .GetResult();
            if (user == null)
                return result.AddError(String.Empty, "User Not Found");

            try
            {
                var userActivity = new UserActivity()
                {
                    UserId = userId,
                    Operation = operation,
                    Text = text
                };
                _dbContext.UserActivities.Add(userActivity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
                await AddLogEntry(ex.GetBaseException().ToString(), ex.ToString());
            }

            return result;
        }

        public async Task<ServiceResult> AddLogEntry(
            string eventName,
            string content,
            int logLevel = 0,
            string userId = "",
            bool isPresentable = false)
        {
            var result = new ServiceResult();

            try
            {
                var logEntry = new LogEntry()
                {
                    UserId = userId,
                    EventName = eventName,
                    Content = content,
                    IsPresentable = isPresentable,
                    LogLevel = logLevel
                };
                _dbContext.LogEntries.Add(logEntry);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
            }

            return result;
        }

        public async Task<GetLogEntriesServiceResult> GetLogEntries(int fromIndex = 0, int limit = 20)
        {
            var result = new GetLogEntriesServiceResult();

            try
            {
                var list = await _dbContext.LogEntries.Where(r =>
                    (fromIndex <= 0 || r.Id < fromIndex)
                ).OrderByDescending(r => r.Id).Take(limit).ToListAsync();
                result.LogEntries = list;
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetBaseException().ToString(), ex.ToString());
                await AddLogEntry(ex.GetBaseException().ToString(), ex.ToString());
            }

            return result;
        }
    }
}