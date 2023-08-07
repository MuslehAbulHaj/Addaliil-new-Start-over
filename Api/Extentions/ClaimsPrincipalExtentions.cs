using System.Security.Claims;

namespace Api.Extentions
{
    public static class ClaimsPrincipalExtentions // this Class is used to get the logged in user details (UserName)
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}