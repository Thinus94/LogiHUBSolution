using System.Security.Claims;

namespace LogiHUB.API.Helpers
{
    public static class GetClient
    {
        public static Guid GetClientId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new UnauthorizedAccessException("ClientId claim not found.");

            return Guid.Parse(claim.Value);
        }
    }
}