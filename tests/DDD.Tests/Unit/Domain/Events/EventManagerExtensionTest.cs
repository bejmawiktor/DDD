using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using Utils.Disposable;

namespace DDD.Tests.Unit.Domain.Events;

[NotInParallel]
internal class EventManagerExtensionTest
{
    [After(Test)]
    public void ClearEventManager() => EventManager.Instance.Dispatcher = null;

    [Test]
    public async Task TestUseCompositeDispatcher_WhenNotUsedPreviously_ThenCompositeDispatcherIsSet()
    {
        EventManager.Instance.UseCompositeDispatcher();

        using (Assert.Multiple())
        {
            _ = await Assert.That(EventManager.Instance.Dispatcher).IsNotNull();
            _ = await Assert
                .That(EventManager.Instance.Dispatcher)
                .IsTypeOf<CompositeEventDispatcher>();
        }
    }

    [Test]
    public async Task TestUseCompositeDispatcher_WhenUsedPreviously_ThenNewCompositeDispatcherIsSet()
    {
        EventManager.Instance.UseCompositeDispatcher();
        IDispatcher<IEvent>? eventDispatcher = EventManager.Instance.Dispatcher;

        EventManager.Instance.UseCompositeDispatcher();

        _ = await Assert
            .That(EventManager.Instance.Dispatcher)
            .IsNotSameReferenceAs(eventDispatcher);
    }

    [Test]
    public async Task TestUseCompositeDispatcher_WhenConfigurationGiven_ThenDispatchersAreSetToEventManagerDispatcher()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    firstDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    secondDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    thirdDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configuration
                .WithDispatcher(firstEventDispatcherMock.Object)
                .WithDispatcher(secondEventDispatcherMock.Object)
                .WithDispatcher(thirdEventDispatcherMock.Object)
        );

        EventManager.Instance.Notify(@event);

        using (Assert.Multiple())
        {
            _ = await Assert.That(firstDispatchedEvent).IsSameReferenceAs(@event);
            _ = await Assert.That(secondDispatchedEvent).IsSameReferenceAs(@event);
            _ = await Assert.That(thirdDispatchedEvent).IsSameReferenceAs(@event);
        }
    }

    [Test]
    public async Task TestUseCompositeDispatcher_WhenNextConfigurationGiven_ThenDispatchersAreReplacedInEventManagerDispatcher()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub? fourthDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    firstDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    secondDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    thirdDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> fourthEventDispatcherMock = new();
        _ = fourthEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    fourthDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configuration
                .WithDispatcher(firstEventDispatcherMock.Object)
                .WithDispatcher(secondEventDispatcherMock.Object)
        );
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configuration
                .WithDispatcher(thirdEventDispatcherMock.Object)
                .WithDispatcher(fourthEventDispatcherMock.Object)
        );

        EventManager.Instance.Notify(@event);

        using (Assert.Multiple())
        {
            _ = await Assert.That(firstDispatchedEvent).IsNull();
            _ = await Assert.That(secondDispatchedEvent).IsNull();
            _ = await Assert.That(thirdDispatchedEvent).IsSameReferenceAs(@event);
            _ = await Assert.That(fourthDispatchedEvent).IsSameReferenceAs(@event);
        }
    }

    [Test]
    public async Task TestUseCompositeDispatcher_WhenNullDispatcherGiven_ThenNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            EventManager.Instance.UseCompositeDispatcher(configuration =>
                configuration.WithDispatcher(null!)
            )
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("dispatcher");
    }
}
