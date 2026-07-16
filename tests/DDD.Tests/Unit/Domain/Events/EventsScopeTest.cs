using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;

namespace DDD.Tests.Unit.Domain.Events;

[NotInParallel]
public class EventsScopeTest
{
    [After(Test)]
    public void ClearEventManager()
    {
        EventManager.CurrentScope?.Dispose();
        EventManager.Instance.Dispatcher = null;
    }

    [Test]
    public async Task TestConstructor_WhenCreating_ThenCurrentScopeFromEventManagerIsSetToCreatedScope()
    {
        EventsScope eventScope = new();

        await Assert.That(EventManager.CurrentScope).IsSameReferenceAs(eventScope);
    }

    [Test]
    public async Task TestAddEvent_WhenNullEventGiven_ThenArgumentNullExceptionIsThrown()
    {
        using EventsScope eventScope = new();

        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(
            () => eventScope.Add(null!)
        );

        await Assert.That(exception!.ParamName).IsEqualTo("item");
    }

    [Test]
    public async Task TestPublish_WhenPublishingWithParentEventScope_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
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
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using (EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            childEventScope.Publish();
        }

        parentEventScope.Publish();

        await Assert.That(events.Count).IsEqualTo(1);
    }

    [Test]
    public async Task TestPublish_WhenDispatcherIsUninitialized_ThenInvalidOperetionExceptionIsThrown()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;

        using EventsScope eventScope = new();
        eventScope.Add(@event);

        InvalidOperationException? exception = Assert.Throws<InvalidOperationException>(
            eventScope.Publish
        );

        await Assert.That(exception!.Message).IsEqualTo("Dispatcher is uninitialized.");
    }

    [Test]
    public async Task TestPublish_WhenMultipleNestedEventsScopesGiven_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
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
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using (EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            using (EventsScope nestedChildEventScope = new())
            {
                nestedChildEventScope.Add(@event);

                nestedChildEventScope.Publish();
            }

            childEventScope.Publish();
        }

        using (EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            childEventScope.Publish();
        }

        await Assert.That(events.Count).IsZero();

        parentEventScope.Publish();

        await Assert.That(events.Count).IsEqualTo(3);
    }

    [Test]
    public async Task TestPublish_WhenPublishingWithoutParentEventScope_ThenEventsAreDispatched()
    {
        bool dispatched = false;
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<EventStub>()))
            .Callback(() => dispatched = true);
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;
        EventStub @event = new();
        EventsScope? eventsScope = null;

        using (eventsScope = new EventsScope())
        {
            eventsScope.Add(@event);

            eventsScope.Publish();
        }

        await Assert.That(dispatched).IsTrue();
    }

    [Test]
    public async Task TestPublishAsync_WhenPublishingWithParentEventScope_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
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
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using (EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            await childEventScope.PublishAsync();
        }

        parentEventScope.Publish();

        await Assert.That(events.Count).IsEqualTo(1);
    }

    [Test]
    public async Task TestPublishAsync_WhenDispatcherIsUninitialized_ThenInvalidOperetionExceptionIsThrown()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;

        using EventsScope eventScope = new();
        eventScope.Add(@event);

        InvalidOperationException? exception = await Assert.ThrowsAsync<InvalidOperationException>(
            eventScope.PublishAsync
        );

        await Assert.That(exception!.Message).IsEqualTo("Dispatcher is uninitialized.");
    }

    [Test]
    public async Task TestPublishAsync_WhenMultipleNestedEventsScopesGiven_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
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
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using (EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            using (EventsScope nestedChildEventScope = new())
            {
                nestedChildEventScope.Add(@event);

                await nestedChildEventScope.PublishAsync();
            }

            await childEventScope.PublishAsync();
        }

        using (EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            await childEventScope.PublishAsync();
        }

        await Assert.That(events.Count).IsZero();

        parentEventScope.Publish();

        await Assert.That(events.Count).IsEqualTo(3);
    }

    [Test]
    public async Task TestPublishAsync_WhenPublishingWithoutParentEventScope_ThenEventsAreDispatched()
    {
        bool dispatched = false;
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
            .Callback(() => dispatched = true);
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;
        EventStub @event = new();
        EventsScope? eventsScope = null;

        using (eventsScope = new EventsScope())
        {
            eventsScope.Add(@event);

            await eventsScope.PublishAsync();
        }

        await Assert.That(dispatched).IsTrue();
    }

    [Test]
    public async Task TestDispose_WhenDisposingCurrentScope_ThenCurrentScopeIsNull()
    {
        using (EventsScope eventsScope = new()) { }

        await Assert.That(EventManager.CurrentScope).IsNull();
    }

    [Test]
    public async Task TestDispose_WhenParentScopeIsDisposedBeforeChildIsDisposed_ThenInvalidOperationExceptionIsThrown()
    {
        Mock<IEvent> eventMock = new();
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock.Setup(e => e.Dispatch(It.IsAny<IEvent>()));
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using EventsScope nestedChildEventScope = new();
        nestedChildEventScope.Add(@event);

        InvalidOperationException? exception = Assert.Throws<InvalidOperationException>(
            parentEventScope.Dispose
        );

        await Assert.That(exception!.Message).IsEqualTo("Scope nested incorrectly.");
    }
}
