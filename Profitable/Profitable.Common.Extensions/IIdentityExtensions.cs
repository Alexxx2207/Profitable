using System.Security.Claims;
using System.Security.Principal;

namespace Profitable.Common.Extensions
{
    public static class IIdentityExtensions
    {
        public static Guid GetUserId(this IIdentity source)
        {
            var IdentityClaims = (ClaimsIdentity)source;
            var claimId = IdentityClaims.FindFirst(ClaimTypes.NameIdentifier);

            if (claimId == null)
            {
                return Guid.Empty;
            }
            else
            {
                return new Guid(claimId.Value);
            }
        }
        
        public static string GetUserEmail(this IIdentity source)
        {
            var IdentityClaims = (ClaimsIdentity)source;
            var claimEmail = IdentityClaims.FindFirst(ClaimTypes.Email);

            if (claimEmail == null)
            {
                return string.Empty;
            }
            else
            {
                return claimEmail.Value;
            }
        }
    }
}