namespace OfflineMessaging.Utils
{
    public class Constants
    {
        public static readonly string JwtKey = AppConfig.Instance.AppSettings.JwtKey;
        public static readonly string JwtIssuer = AppConfig.Instance.AppSettings.JwtIssuer;
        public static readonly int JwtExpireDays = AppConfig.Instance.AppSettings.JwtExpireDays;
        public static readonly string ConnectionString = AppConfig.Instance.AppSettings.ConnectionString;
    }

    public class EventConstants
    {
        public static string Login = "EVENT_LOGIN";
        public static string LoginAttempt = "EVENT_LOGIN_ATTEMPT";
        public static string Register = "EVENT_REGISTER";
        public static string BlockUser = "EVENT_BLOCK_USER";
        public static string UnblockUser = "EVENT_UNBLOCK_USER";
    }
}