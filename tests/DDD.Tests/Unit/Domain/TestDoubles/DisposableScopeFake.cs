using DDD.Domain.Utils;

namespace DDD.Tests.Unit.Domain.TestDoubles;

internal class DisposableScopeFake
    : DisposableScope<DisposableScopeFake, DisposableScopeContainerFake>
{
    public string Text { get; set; } = "Text";

    public override void Clear() => this.Text = "";
}
