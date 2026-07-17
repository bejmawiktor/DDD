using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using MediatR;
using Moq;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

[NotInParallel]
public class CompositeEventManagerConfigurationExtensionTest
{
    [After(Test)]
    public void ClearEventManager() => EventManager.Instance.Dispatcher = null;

    [Test]
    public async Task TestWithMediatRDispatcher_WhenMultipleDispatchersAdded_ThenMultipleDispatchAreExecuted()
    {
        EventNotification<EventStub>? firstDispatchedEvent = null;
        EventNotification<EventStub>? secondDispatchedEvent = null;
        EventNotification<EventStub>? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IMediator> firstEventMediatorMock = new();
        _ = firstEventMediatorMock
            .Setup(e => e.Publish(It.IsAny<INotification>(), default))
            .Returns(
                async (INotification notification, CancellationToken token) =>
                {
                    await Task.Run(() =>
                    {
                        firstDispatchedEvent = notification as EventNotification<EventStub>;
                    });
                }
            );
        Mock<IMediator> secondEventMediatorMock = new();
        _ = secondEventMediatorMock
            .Setup(e => e.Publish(It.IsAny<INotification>(), default))
            .Returns(
                async (INotification notification, CancellationToken token) =>
                {
                    await Task.Run(() =>
                    {
                        secondDispatchedEvent = notification as EventNotification<EventStub>;
                    });
                }
            );
        Mock<IMediator> thirdEventMediatorMock = new();
        _ = thirdEventMediatorMock
            .Setup(e => e.Publish(It.IsAny<INotification>(), default))
            .Returns(
                async (INotification notification, CancellationToken token) =>
                {
                    await Task.Run(() =>
                    {
                        thirdDispatchedEvent = notification as EventNotification<EventStub>;
                    });
                }
            );
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configuration
                .WithMediatRDispatcher(firstEventMediatorMock.Object)
                .WithMediatRDispatcher(secondEventMediatorMock.Object)
                .WithMediatRDispatcher(thirdEventMediatorMock.Object)
        );

        EventManager.Instance.Notify(@event);

        using (Assert.Multiple())
        {
            _ = await Assert.That(firstDispatchedEvent?.Event).IsSameReferenceAs(@event);
            _ = await Assert.That(secondDispatchedEvent?.Event).IsSameReferenceAs(@event);
            _ = await Assert.That(thirdDispatchedEvent?.Event).IsSameReferenceAs(@event);
        }
    }

    [Test]
    public async Task TestWithMediatRDispatcher_WhenNullMediatorGiven_ThenArgumentNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            new CompositeEventDispatcherConfiguration().WithMediatRDispatcher(null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("mediator");
    }
}
