using Skeleton.Core.Contracts;

namespace Skeleton.Core.Exceptions;

public sealed class BusinessRuleException(
    string field,
    string message) : Exception(message), IBusinessRuleException
{
    public string Field { get; } = field;
}
