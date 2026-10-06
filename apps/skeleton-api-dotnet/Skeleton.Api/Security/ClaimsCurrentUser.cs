using System.Security.Claims;
using Skeleton.Core.Contracts;

namespace Skeleton.Api.Security;

public sealed class ClaimsCurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    // Also used outside this class (e.g. RateLimitingRegistration) to partition by caller
    // without needing a full ICurrentUser/DI resolution in that context.
    public const string ObjectIdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";

    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public Guid UserId
    {
        get
        {
            var objectId = Principal?.FindFirstValue(ObjectIdClaimType)
                           ?? throw new InvalidOperationException("Missing object id claim.");

            return !Guid.TryParse(objectId, out var userId)
                ? throw new ArgumentException("Object id claim is not a valid GUID.", nameof(objectId))
                : userId;
        }
    }

    public string DisplayName => Principal?.FindFirstValue("name")
                                 ?? throw new InvalidOperationException("Missing name claim.");

    public string Email => Principal?.FindFirstValue("preferred_username")
                           ?? throw new InvalidOperationException("Missing preferred_username claim.");

    public IReadOnlyList<string> Roles =>
        Principal?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList() ?? [];
}
