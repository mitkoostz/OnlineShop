using System.Linq;
using System.Security.Claims;

namespace Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetriveEmailFromPrincipal( this ClaimsPrincipal user){
            return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?
             .Value;
        }
    }
}