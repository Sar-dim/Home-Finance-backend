using Common.Models.Exceptions;
using System.Linq;
using System.Security.Claims;

namespace Common
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == "PersonId")?.Value ?? string.Empty;
        }
    }
}
