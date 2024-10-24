﻿using DDD.Domain.Common;
using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Domain.Events;

[TestFixture]
public class EventManagerTest
{
    private static IScopeHandler<EventsScope, IEvent, EventManager> EventManager =>
        DDD.Domain.Events.EventManager.Instance;

    [TearDown]
    public void ClearEventManager()
    {
        DDD.Domain.Events.EventManager.CurrentScope = null;
        EventManager.Dispatcher = null;
    }

    [Test]
    public void TestEventManager_WhenNoEventScopeCreated_ThenEventsAreDispatchedImmediately()
    {
        bool dispatched = false;
        Mock<IDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        EventManager.Dispatcher = eventDispatcherMock.Object;

        EventManager.Notify(new EventStub());

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestNotify_WhenScopeWasCreated_ThenEventsArentDispatched()
    {
        bool dispatched = false;
        Mock<IEvent> eventMock = new();
        Mock<IDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        EventManager.Dispatcher = eventDispatcherMock.Object;

        using(EventsScope eventsScope = new())
        {
            EventManager.Notify(eventMock.Object);
        }

        Assert.That(dispatched, Is.False);
    }

    [Test]
    public void TestNotify_WhenScopeWasCreated_ThenEventsAreAddedToScopeEvents()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;
        Mock<IDispatcher> eventDispatcherMock = new();
        EventManager.Dispatcher = eventDispatcherMock.Object;

        using EventsScope eventsScope = new();
        EventManager.Notify(eventMock.Object);

        Assert.That(eventsScope.Items.Contains(@event));
    }

    [Test]
    public void TestNotify_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            () => EventManager.Notify(new EventStub())
        );
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasCreated_ThenEventsArentDispatched()
    {
        bool dispatched = false;
        Mock<IEvent> eventMock = new();
        Mock<IDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        EventManager.Dispatcher = eventDispatcherMock.Object;

        using(EventsScope eventsScope = new())
        {
            await EventManager.NotifyAsync(eventMock.Object);
        }

        Assert.That(dispatched, Is.False);
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasCreated_ThenEventsAreAddedToScopeEvents()
    {
        Mock<IEvent> eventMock = new();
        IEvent @event = eventMock.Object;
        Mock<IDispatcher> eventDispatcherMock = new();
        EventManager.Dispatcher = eventDispatcherMock.Object;

        using EventsScope eventsScope = new();
        await EventManager.NotifyAsync(eventMock.Object);

        Assert.That(eventsScope.Items.Contains(@event));
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasntCreated_ThenEventsAreDispatched()
    {
        bool dispatched = false;
        Mock<IDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Callback(() => dispatched = true);
        EventManager.Dispatcher = eventDispatcherMock.Object;

        await EventManager.NotifyAsync(new EventStub());

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestNotifyAsync_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            async () => await EventManager.NotifyAsync(new EventStub())
        );
    }
}