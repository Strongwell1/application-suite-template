namespace Skeleton.Core.Contracts;

public interface ICurrentUser
{
    Guid UserId { get; }
    string DisplayName { get; }
    string Email { get; }
    IReadOnlyList<string> Roles { get; }
}
