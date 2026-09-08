namespace Skeleton.Core.Domain.Shared;

public sealed record Actor
{
    private Actor() //  EF
    {
        DisplayName = null!;
        Email = null!;
    }

    public Actor(Guid id, string displayName, string emailAddress)
    {
        Id = Guard.NotEmpty(id, nameof(id));
        DisplayName = Guard.TrimmedRequired(displayName, nameof(displayName), ModelConstraints.MaxStringLengthDefault);
        Email = Guard.Email(emailAddress, nameof(emailAddress), ModelConstraints.MaxEmailLength);
    }

    public Guid Id { get; }
    public string DisplayName { get; }
    public string Email { get; }
}
