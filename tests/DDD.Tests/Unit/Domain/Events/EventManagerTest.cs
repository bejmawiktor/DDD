using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;
using Utils.Disposable;

namespace DDD.Tests.Unit.Domain.Events;

[TestFixture]
public class EventManagerTest
{
    [TearDown]
    public void ClearEventManager()
    {
        DDD.Domain.Events.EventManager.CurrentScope?.Dispose();
        DDD.Domain.Events.EventManager.Instance.Dispatcher = null;
    }

    [Test]
    public void TestEventManager_WhenNoEventScopeCreated_ThenEventsAreDispatchedImmediately()
    {
        bool dispatched = false;
        Mock<IDispatcher<IEvent>> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        DDD.Domain.Events.EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        EventManager.Instance.Notify(new EventStub());

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestNotify_WhenScopeWasCreated_ThenEventsArentDispatched()
    {
        bool dispatched = false;
        Mock<IEvent> eventMock = new();
        Mock<IDispatcher<IEvent>> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        DDD.Domain.Events.EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using (EventsScope eventsScope = new())
        {
            EventManager.Instance.Notify(eventMock.Object);
        }

        Assert.That(dispatched, Is.False);
    }

    [Test]
    public void TestNotify_WhenScopeWasCreated_ThenEventsAreDispatchedOnPublish()
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
        DDD.Domain.Events.EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope eventsScope = new();
        EventManager.Instance.Notify(eventMock.Object);

        Assert.That(events.Contains(@event), Is.False);

        eventsScope.Publish();

        Assert.That(events.Contains(@event), Is.True);
    }

    [Test]
    public void TestNotify_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            () => EventManager.Instance.Notify(new EventStub())
        );
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
        DDD.Domain.Events.EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using (EventsScope eventsScope = new())
        {
            await EventManager.Instance.NotifyAsync(eventMock.Object);
        }

        Assert.That(dispatched, Is.False);
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
        DDD.Domain.Events.EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using EventsScope eventsScope = new();
        await EventManager.Instance.NotifyAsync(eventMock.Object);

        Assert.That(events.Contains(@event), Is.False);

        eventsScope.Publish();

        Assert.That(events.Contains(@event), Is.True);
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasntCreated_ThenEventsAreDispatched()
    {
        bool dispatched = false;
        Mock<IDispatcher<IEvent>> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        DDD.Domain.Events.EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        await EventManager.Instance.NotifyAsync(new EventStub());

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestNotifyAsync_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            async () => await EventManager.Instance.NotifyAsync(new EventStub())
        );
    }
}
