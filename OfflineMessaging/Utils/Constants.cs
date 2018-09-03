namespace OfflineMessaging.Utils
{
    public class Constants
    {
        public const string JwtKey = "_guclu_bi_sifre_yaz_daha_sonra_buraya_";
        public const string JwtIssuer = "offlinemessaging";
        public const int JwtExpireDays = 30;
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