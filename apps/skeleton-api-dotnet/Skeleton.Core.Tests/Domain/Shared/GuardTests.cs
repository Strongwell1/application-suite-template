using Skeleton.Core.Domain.Shared;

namespace Skeleton.Core.Tests.Domain.Shared;

public class GuardTests
{
    [Fact]
    public void TrimmedRequired_TrimsSurroundingWhitespace()
    {
        var result = Guard.TrimmedRequired("  hello  ", "value", 50);

        Assert.Equal("hello", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void TrimmedRequired_ThrowsForNullEmptyOrWhitespace(string? value)
    {
        Assert.ThrowsAny<ArgumentException>(() => Guard.TrimmedRequired(value, "value", 50));
    }

    [Fact]
    public void TrimmedRequired_ThrowsWhenLongerThanMaxLength()
    {
        Assert.Throws<ArgumentException>(() => Guard.TrimmedRequired("hello", "value", 3));
    }

    [Theory]
    [InlineData("person@example.com")]
    [InlineData("first.last@sub.example.co")]
    public void Email_AcceptsValidAddresses(string email)
    {
        var result = Guard.Email(email, "email", ModelConstraints.MaxEmailLength);

        Assert.Equal(email, result);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing-domain@")]
    [InlineData("@missing-local.com")]
    [InlineData("two@at@signs.com")]
    [InlineData("trailing.dot@example.")]
    public void Email_RejectsInvalidAddresses(string email)
    {
        Assert.Throws<ArgumentException>(() => Guard.Email(email, "email", ModelConstraints.MaxEmailLength));
    }

    [Fact]
    public void NotEmpty_Guid_ThrowsForEmptyGuid()
    {
        Assert.Throws<ArgumentException>(() => Guard.NotEmpty(Guid.Empty, "id"));
    }

    [Fact]
    public void NotEmpty_Guid_ReturnsValueWhenNotEmpty()
    {
        var id = Guid.NewGuid();

        var result = Guard.NotEmpty(id, "id");

        Assert.Equal(id, result);
    }
}
