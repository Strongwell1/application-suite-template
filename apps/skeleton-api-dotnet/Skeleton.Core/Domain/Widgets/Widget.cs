using Skeleton.Core.Domain.Shared;

namespace Skeleton.Core.Domain.Widgets;

// Aggregate root — replace with real domain logic.
// Create/UpdateDetails/Delete are intentionally unimplemented; they exist to show the
// shape (factory method + explicit mutation methods, no public setters) the next entity
// should follow.
public class Widget
{
    private Widget() { } // EF

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public WidgetState State { get; private set; }
    public Actor CreatedBy { get; private set; } = null!;
    public DateTime CreatedUtc { get; private set; }
    public DateTime? UpdatedUtc { get; private set; }
    public Actor? DeletedBy { get; private set; }
    public string? DeletedReason { get; private set; }
    public DateTime? DeletedUtc { get; private set; }

    public static Widget Create(Guid id, string name, string? description, Actor createdBy)
    {
        throw new NotImplementedException();
    }

    public void UpdateDetails(string name, string? description, DateTime updatedUtc)
    {
        throw new NotImplementedException();
    }

    public void Delete(Actor deletedBy, string? reason, DateTime deletedUtc)
    {
        throw new NotImplementedException();
    }
}
