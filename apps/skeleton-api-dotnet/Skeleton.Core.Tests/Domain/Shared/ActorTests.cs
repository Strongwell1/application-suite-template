using Skeleton.Core.Domain.Shared;

namespace Skeleton.Core.Tests.Domain.Shared;

public class ActorTests
{
    [Fact]
    public void Constructor_TrimsDisplayNameAndKeepsValidValues()
    {
        var id = Guid.NewGuid();

        var actor = new Actor(id, "  Jane Doe  ", "jane.doe@example.com");

        Assert.Equal(id, actor.Id);
        Assert.Equal("Jane Doe", actor.DisplayName);
        Assert.Equal("jane.doe@example.com", actor.Email);
    }

    [Fact]
    public void Constructor_ThrowsForEmptyId()
    {
        Assert.Throws<ArgumentException>(() => new Actor(Guid.Empty, "Jane Doe", "jane.doe@example.com"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ThrowsForBlankDisplayName(string displayName)
    {
        Assert.ThrowsAny<ArgumentException>(() => new Actor(Guid.NewGuid(), displayName, "jane.doe@example.com"));
    }

    [Fact]
    public void Constructor_ThrowsForInvalidEmail()
    {
        Assert.Throws<ArgumentException>(() => new Actor(Guid.NewGuid(), "Jane Doe", "not-an-email"));
    }

    [Fact]
    public void Actors_WithSameValues_AreEqual()
    {
        // Actor is a record, so value equality is expected — useful when asserting
        // "who did this" fields in a use-case test without reference-comparing.
        var id = Guid.NewGuid();

        var first = new Actor(id, "Jane Doe", "jane.doe@example.com");
        var second = new Actor(id, "Jane Doe", "jane.doe@example.com");

        Assert.Equal(first, second);
    }
}
