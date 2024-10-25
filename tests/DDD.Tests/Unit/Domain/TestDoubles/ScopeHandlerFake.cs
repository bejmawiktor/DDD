using DDD.Domain.Common;

namespace DDD.Tests.Unit.Domain.TestDoubles;

internal class ScopeHandlerFake : ScopeHandler<ScopeFake, string, ScopeHandlerFake>
{
    public override IDispatcher? Dispatcher { get; set; }

    public ScopeHandlerFake() { }
}
