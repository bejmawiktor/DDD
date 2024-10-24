using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Domain.Events;

[TestFixture]
public class EventsScopeTest
{
    [TearDown]
    public void ClearEventManager()
    {
        EventManager.CurrentScope = null;
        EventManager.Instance.Dispatcher = null;
    }

    [Test]
    public void TestConstructor_WhenCreating_ThenEventsListIsSetAsEmpty()
    {
        EventsScope eventScope = new();

        Assert.That(eventScope.Items, Is.Empty);
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
    public void TestAddEvent_WhenEventGiven_ThenEventIsAdded()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;
        EventsScope? eventsScope;
        using(eventsScope = new())
        {
            eventsScope.Add(@event);

            Assert.That(eventsScope.Items.Contains(@event));
        }
    }

    [Test]
    public void TestPublish_WhenPublishingWithParentEventScope_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock.Setup(e => e.Dispatch(It.IsAny<IEvent>()));
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using(EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            childEventScope.Publish();
        }

        Assert.That(parentEventScope.Items.Count, Is.EqualTo(1));
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
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock.Setup(e => e.Dispatch(It.IsAny<IEvent>()));
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using(EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            using(EventsScope nestedChildEventScope = new())
            {
                nestedChildEventScope.Add(@event);

                nestedChildEventScope.Publish();
            }

            childEventScope.Publish();
        }

        using(EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            childEventScope.Publish();
        }

        Assert.That(parentEventScope.Items.Count, Is.EqualTo(3));
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

        using(eventsScope = new EventsScope())
        {
            eventsScope.Add(@event);

            eventsScope.Publish();
        }

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestPublish_WhenPublishing_ThenEventsAreCleared()
    {
        Mock<IEvent> eventMock = new();
        Mock<IEventDispatcher> eventDispatcherMock = new();
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;
        IEvent @event = eventMock.Object;
        EventsScope? eventsScope;
        using(eventsScope = new EventsScope())
        {
            eventsScope.Add(@event);

            eventsScope.Publish();
        }

        Assert.That(eventsScope.Items, Is.Empty);
    }

    [Test]
    public async Task TestPublishAsync_WhenPublishingWithParentEventScope_ThenEventsAreAddedToParentEventScope()
    {
        Mock<IEvent> eventMock = new();
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock.Setup(e => e.DispatchAsync(It.IsAny<IEvent>()));
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using(EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            await childEventScope.PublishAsync();
        }

        Assert.That(parentEventScope.Items.Count, Is.EqualTo(1));
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
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock.Setup(e => e.DispatchAsync(It.IsAny<IEvent>()));
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope parentEventScope = new();
        using(EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            using(EventsScope nestedChildEventScope = new())
            {
                nestedChildEventScope.Add(@event);

                await nestedChildEventScope.PublishAsync();
            }

            await childEventScope.PublishAsync();
        }

        using(EventsScope childEventScope = new())
        {
            childEventScope.Add(@event);

            await childEventScope.PublishAsync();
        }

        Assert.That(parentEventScope.Items.Count, Is.EqualTo(3));
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

        using(eventsScope = new EventsScope())
        {
            eventsScope.Add(@event);

            await eventsScope.PublishAsync();
        }

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public async Task TestPublishAsync_WhenPublishing_ThenEventsAreCleared()
    {
        Mock<IEvent> eventMock = new();
        Mock<IEventDispatcher> eventDispatcherMock = new();
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;
        IEvent @event = eventMock.Object;
        EventsScope? eventsScope;

        using(eventsScope = new EventsScope())
        {
            eventsScope.Add(@event);

            await eventsScope.PublishAsync();
        }

        Assert.That(eventsScope.Items, Is.Empty);
    }

    [Test]
    public void TestClear_WhenClearing_ThenEventsAreEmpty()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;
        EventsScope? eventsScope;

        using(eventsScope = new EventsScope())
        {
            eventsScope.Add(@event);

            eventsScope.Clear();
        }

        Assert.That(eventsScope.Items, Is.Empty);
    }

    [Test]
    public void TestDispose_WhenDisposingCurrentScope_ThenCurrentScopeIsNull()
    {
        using(EventsScope eventsScope = new())
        {
        }

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

    [Test]
    public void TestDispose_WhenParentScopeIsDisposedBeforeChildIsDisposed_ThenEventsAreCleared()
    {
        Mock<IEvent> eventMock = new();
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock.Setup(e => e.Dispatch(It.IsAny<IEvent>()));
        IEvent @event = eventMock.Object;
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;
        EventsScope? parentEventScope = null;

        try
        {
            using(parentEventScope = new EventsScope())
            {
                using EventsScope nestedChildEventScope = new();
                nestedChildEventScope.Add(@event);

                parentEventScope.Dispose();
            }
        }
        catch(InvalidOperationException) { }

        Assert.That(parentEventScope?.Items, Is.Empty);
    }
}