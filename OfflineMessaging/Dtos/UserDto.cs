using System;

namespace OfflineMessaging.Dtos
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public DateTime? RegisterDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        
        public string DateOfBirth { get; set; }
        public string UserName { get; set; }
    }
}