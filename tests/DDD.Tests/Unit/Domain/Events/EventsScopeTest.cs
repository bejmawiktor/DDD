using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events;

[TestFixture]
public class EventsScopeTest
{
    [TearDown]
    public void ClearEventManager()
    {
        EventManager.CurrentScope?.Dispose();
        EventManager.Instance.Dispatcher = null;
    }

    [Test]
    public void TestConstructor_WhenCreating_ThenCurrentScopeFromEventManagerIsSetToCreatedScope()
    {
        EventsScope eventScope = new();

        Assert.That(EventManager.CurrentScope, Is.SameAs(eventScope));
    }

    [Test]
    public void TestAddEvent_WhenNullEventGiven_ThenArgumentNullExceptionIsThrown()
    {
        using EventsScope eventScope = new();
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("item"),
            () => eventScope.Add(null!)
        );
    }

    [Test]
    public void TestPublish_WhenPublishingWithParentEventScope_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
        List<IEvent> events = new List<IEvent>();
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

        Assert.That(events.Count, Is.EqualTo(1));
    }

    [Test]
    public void TestPublish_WhenDispatcherIsUninitialized_ThenInvalidOperetionExceptionIsThrown()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;

        using EventsScope eventScope = new();
        eventScope.Add(@event);

        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            eventScope.Publish
        );
    }

    [Test]
    public void TestPublish_WhenMultipleNestedEventsScopesGiven_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
        List<IEvent> events = new List<IEvent>();
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

        Assert.That(events.Count, Is.Zero);

        parentEventScope.Publish();

        Assert.That(events.Count, Is.EqualTo(3));
    }

    [Test]
    public void TestPublish_WhenPublishingWithoutParentEventScope_ThenEventsAreDispatched()
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

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public async Task TestPublishAsync_WhenPublishingWithParentEventScope_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
        List<IEvent> events = new List<IEvent>();
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

        Assert.That(events.Count, Is.EqualTo(1));
    }

    [Test]
    public void TestPublishAsync_WhenDispatcherIsUninitialized_ThenInvalidOperetionExceptionIsThrown()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;

        using EventsScope eventScope = new();
        eventScope.Add(@event);

        _ = Assert.ThrowsAsync(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            eventScope.PublishAsync
        );
    }

    [Test]
    public async Task TestPublishAsync_WhenMultipleNestedEventsScopesGiven_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
        List<IEvent> events = new List<IEvent>();
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

        Assert.That(events.Count, Is.Zero);

        parentEventScope.Publish();

        Assert.That(events.Count, Is.EqualTo(3));
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

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestDispose_WhenDisposingCurrentScope_ThenCurrentScopeIsNull()
    {
        using (EventsScope eventsScope = new()) { }

        Assert.That(EventManager.CurrentScope, Is.Null);
    }

    [Test]
    public void TestDispose_WhenParentScopeIsDisposedBeforeChildIsDisposed_ThenInvalidOperationExceptionIsThrown()
    {
        Mock<IEvent> eventMock = new();
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock.Setup(e => e.Dispatch(It.IsAny<IEvent>()));
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using EventsScope nestedChildEventScope = new();
        nestedChildEventScope.Add(@event);

        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Scope nested incorrectly."),
            parentEventScope.Dispose
        );
    }
}
