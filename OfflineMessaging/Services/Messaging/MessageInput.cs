using System.ComponentModel.DataAnnotations;

namespace OfflineMessaging.Services.Messaging
{
    public class MessageInput
    {
        [Required] public string Username { get; set; }
        [Required] public string Text { get; set; }
    }
}