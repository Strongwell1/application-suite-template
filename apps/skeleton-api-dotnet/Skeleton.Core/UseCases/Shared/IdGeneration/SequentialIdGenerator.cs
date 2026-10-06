using RT.Comb;

namespace Skeleton.Core.UseCases.Shared.IdGeneration;

public sealed class SequentialIdGenerator : IIdGenerator
{
    private static readonly SqlCombProvider Provider = new(new SqlDateTimeStrategy());

    public Guid New()
    {
        return Provider.Create();
    }
}
