using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using Utils.Disposable;

namespace DDD.Tests.Unit.Domain.Events;

[NotInParallel]
public class EventManagerTest
{
    [After(Test)]
    public void ClearEventManager()
    {
        EventManager.CurrentScope?.Dispose();
        EventManager.Instance.Dispatcher = null;
    }

    [Test]
    public async Task TestEventManager_WhenNoEventScopeCreated_ThenEventsAreDispatchedImmediately()
    {
        bool dispatched = false;
        Mock<IDispatcher<IEvent>> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        EventManager.Instance.Notify(new EventStub());

        _ = await Assert.That(dispatched).IsTrue();
    }

    [Test]
    public async Task TestNotify_WhenScopeWasCreated_ThenEventsArentDispatched()
    {
        bool dispatched = false;
        Mock<IEvent> eventMock = new();
        Mock<IDispatcher<IEvent>> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using (EventsScope eventsScope = new())
        {
            EventManager.Instance.Notify(eventMock.Object);
        }

        _ = await Assert.That(dispatched).IsFalse();
    }

    [Test]
    public async Task TestNotify_WhenScopeWasCreated_ThenEventsAreDispatchedOnPublish()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;
        List<IEvent> events = [];
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent e) =>
                {
                    events.Add(e);
                }
            );
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope eventsScope = new();
        EventManager.Instance.Notify(eventMock.Object);

        _ = await Assert.That(events.Contains(@event)).IsFalse();

        eventsScope.Publish();

        _ = await Assert.That(events.Contains(@event)).IsTrue();
    }

    [Test]
    public async Task TestNotify_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        InvalidOperationException? exception = Assert.Throws<InvalidOperationException>(() =>
            EventManager.Instance.Notify(new EventStub())
        );

        _ = await Assert.That(exception!.Message).IsEqualTo("Dispatcher is uninitialized.");
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasCreated_ThenEventsArentDispatched()
    {
        bool dispatched = false;
        Mock<IEvent> eventMock = new();
        Mock<IDispatcher<IEvent>> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using (EventsScope eventsScope = new())
        {
            await EventManager.Instance.NotifyAsync(eventMock.Object);
        }

        _ = await Assert.That(dispatched).IsFalse();
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasCreated_ThenEventsAreDispatchedOnPublish()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;
        List<IEvent> events = [];
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent e) =>
                {
                    events.Add(e);
                }
            );
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope eventsScope = new();
        await EventManager.Instance.NotifyAsync(eventMock.Object);

        _ = await Assert.That(events.Contains(@event)).IsFalse();

        eventsScope.Publish();

        _ = await Assert.That(events.Contains(@event)).IsTrue();
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasntCreated_ThenEventsAreDispatched()
    {
        bool dispatched = false;
        Mock<IDispatcher<IEvent>> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        await EventManager.Instance.NotifyAsync(new EventStub());

        _ = await Assert.That(dispatched).IsTrue();
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        InvalidOperationException? exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () =>
                await EventManager.Instance.NotifyAsync(new EventStub())
        );

        _ = await Assert.That(exception!.Message).IsEqualTo("Dispatcher is uninitialized.");
    }
}
