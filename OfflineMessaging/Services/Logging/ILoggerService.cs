using System.Threading.Tasks;
using OfflineMessaging.Entities;

namespace OfflineMessaging.Services.Logging
{
    public interface ILoggerService
    {
        Task<ServiceResult> AddUserActivity(string userId, string operation, string text);

        Task<ServiceResult> AddLogEntry(string eventName, string content, int logLevel = 0, string userId = "",
            bool isPresentable = false);
        Task<GetLogEntriesServiceResult> GetLogEntries(int fromIndex = 0, int limit = 20);
    }
}