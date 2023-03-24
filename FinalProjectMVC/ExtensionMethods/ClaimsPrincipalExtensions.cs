using System.Linq;
using System.Security.Claims;

namespace FinalProjectMVC.ExtensionMethods
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsInAnyRole(this ClaimsPrincipal principal, params string[] roles)
        {
            return roles.Any(role => principal.IsInRole(role));
        }
    }
}