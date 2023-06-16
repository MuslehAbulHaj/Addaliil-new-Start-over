using System.Security.Claims;

namespace Api.Extentions
{
    public static class ClaimsPrincipalExtentions // this Class is used to get the logged in user details (UserName)
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}