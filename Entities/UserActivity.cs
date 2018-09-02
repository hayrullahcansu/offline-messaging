namespace OfflineMessaging.Entities
{
    public class UserActivity : TimeAwareEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Operation { get; set; }
        public string Text { get; set; }
    }
}