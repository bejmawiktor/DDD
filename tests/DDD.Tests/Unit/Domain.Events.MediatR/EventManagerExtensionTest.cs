using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using MediatR;
using Moq;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

[NotInParallel]
internal class EventManagerExtensionTest
{
    [After(Test)]
    public void ClearEventManager() => EventManager.Instance.Dispatcher = null;

    [Test]
    public async Task TestUseMediatREventDispatcher_WhenMediatorGiven_ThenMediatorIsSet()
    {
        Mock<IMediator> mediatorMock = new();
        IMediator mediator = mediatorMock.Object;

        EventManager.Instance.UseMediatREventDispatcher(mediator);

        _ = await Assert.That(EventManager.Instance.Dispatcher).IsNotNull();
    }

    [Test]
    public async Task TestUseMediatREventDispatcher_WhenNullMediatorGiven_ThenNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            EventManager.Instance.UseMediatREventDispatcher(null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("mediator");
    }
}
