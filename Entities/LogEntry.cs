namespace OfflineMessaging.Entities
{
    public class LogEntry : TimeAwareEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string EventName { get; set; }
        public string Content { get; set; }

        public int LogLevel { get; set; }

        public bool IsPresentable { get; set; } // show to user?

        public static class Level
        {
            public static int Info = 0;
            public static int Danger = 1;
        }
    }
}