using Skeleton.Core.Domain.Shared;

namespace Skeleton.Core.Contracts;

public static class ICurrentUserExtensions
{
    public static Actor ToActor(this ICurrentUser currentUser) =>
        new(currentUser.UserId, currentUser.DisplayName, currentUser.Email);
}
