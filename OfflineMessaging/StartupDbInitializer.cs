using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OfflineMessaging.Entities;

namespace OfflineMessaging
{
    public static class StartupDbInitializer
    {
        public static async Task SeedData(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            dbContext.Database.EnsureCreated();
            await AddUserAsync(dbContext, userManager);
        }

        private static async Task AddUserAsync(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            if (!dbContext.Users.Any())
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@admin.com"
                };
                await userManager.CreateAsync(user, "AdminPassword123.@");

                user = new User
                {
                    UserName = "test",
                    Email = "test@admin.com"
                };
                await userManager.CreateAsync(user, "TestPassword123.@");
                
            }
        }
    }
}