using System.ComponentModel.DataAnnotations;

namespace OfflineMessaging.Services.Account
{
    public class LoginInput
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}