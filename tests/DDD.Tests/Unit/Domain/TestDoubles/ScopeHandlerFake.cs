using Utils.Disposable;

namespace DDD.Tests.Unit.Domain.TestDoubles;

internal class ScopeHandlerFake : ScopeHandler<ScopeFake, string, ScopeHandlerFake>
{
    public override IDispatcher<string>? Dispatcher { get; set; }

    public ScopeHandlerFake() { }
}
