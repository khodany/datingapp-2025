using System.Security.Claims;

namespace API.Extensions;

public static class CalimsPrincipalExtensions
{
    public static string GetMemberID(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier) 
        ?? throw new Exception("Cannot get member id from token");
    }
}
