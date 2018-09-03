namespace OfflineMessaging.Entities
{
    public class Message : TimeAwareEntity
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public string Text { get; set; }
    }
}