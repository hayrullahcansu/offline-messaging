using System.Security.Claims;

namespace OfflineMessaging.Extensions
{
    public static class Extension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            // user.FindFirstValue(ClaimTypes.NameIdentifier);
            //return user.FindFirstValue(ClaimTypes.NameIdentifier);
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}