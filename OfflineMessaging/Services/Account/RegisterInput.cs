using System.ComponentModel.DataAnnotations;

namespace OfflineMessaging.Services.Account
{
    public class RegisterInput
    {
        [Required] public string Username { get; set; }

        [Required] public string Name { get; set; }

        public string Surname { get; set; }

        [Required] public string Email { get; set; }

        [Required] public string Password { get; set; }
    }
}