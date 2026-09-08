using Skeleton.Core.Contracts;

namespace Skeleton.Core.Exceptions;

public sealed class ConflictException(
    string field,
    string message) : Exception(message), IConflictException
{
    public string Field { get; } = field;
}
