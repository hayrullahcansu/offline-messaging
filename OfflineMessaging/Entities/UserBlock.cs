namespace OfflineMessaging.Entities
{
    public class UserBlock : TimeAwareEntity
    {
        public int Id { get; set; }
        public string BlockerId { get; set; }
        public string BlockedId { get; set; }
    }
}