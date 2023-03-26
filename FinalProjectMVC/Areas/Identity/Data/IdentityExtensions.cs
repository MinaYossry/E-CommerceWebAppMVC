using FinalProjectMVC.Constants;
using System.Security.Claims;

namespace FinalProjectMVC.Areas.Identity.Data
{
    public static class IdentityExtensions
    {
        public static bool IsSeller(this ClaimsPrincipal user)
        {
            return user.Identity?.IsAuthenticated == true && user.IsInRole(Roles.Seller.ToString());
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
