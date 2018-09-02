using System;
using Microsoft.AspNetCore.Identity;

namespace OfflineMessaging.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public DateTime? RegisterDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public string VerifyState { get; set; }
        public bool IsVerified => VerifyState == VerifyStates.Completed;

        // public string PhoneNumber { get; set; } User base class already has PhoneNumber property, so no need to add here
        public string DateOfBirth { get; set; }

        public static class VerifyStates
        {
            public static string Pending = "PENDING";
            public static string Completed = "COMPLETED";
            public static string Rejected = "REJECTED";
        }
    }
}