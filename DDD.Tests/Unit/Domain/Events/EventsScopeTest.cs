﻿using DDD.Domain.Events;
using Moq;
using NUnit.Framework;
using System;

namespace DDD.Tests.Unit.Domain.Events
{
    [TestFixture]
    public class EventsScopeTest
    {
        [TearDown]
        public void ClearEventManager()
        {
            EventManager.CurrentScope = null;
            EventManager.Instance.EventDispatcher = null;
        }

        [Test]
        public void TestConstructor_WhenCreating_ThenEventsListIsSetAsEmpty()
        {
            var eventScope = new EventsScope();

            Assert.That(eventScope.Events, Is.Empty);
        }

        [Test]
        public void TestConstructor_WhenCreating_ThenCurrentScopeFromEventManagerIsSetToCreatedScope()
        {
            var eventScope = new EventsScope();

            Assert.That(EventManager.CurrentScope, Is.SameAs(eventScope));
        }

        [Test]
        public void TestAddEvent_WhenNullEventGiven_ThenArgumentNullExceptionIsThrown()
        {
            var eventScope = new EventsScope();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("event"),
                () => eventScope.AddEvent((IEvent)null));
        }

        [Test]
        public void TestAddEvent_WhenEventGiven_ThenEventIsAdded()
        {
            var eventMock = new Mock<IEvent>();
            var eventsScope = new EventsScope();
            var @event = eventMock.Object;

            eventsScope.AddEvent(@event);

            Assert.That(eventsScope.Events.Contains(@event));
        }

        [Test]
        public void TestPublish_WhenPublishingWithParentEventScope_ThenEventsAreAddedToParentEventScope()
        {
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()));
            var @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(var parentEventScope = new EventsScope())
            {
                using(var childEventScope = new EventsScope())
                {
                    childEventScope.AddEvent(@event);

                    childEventScope.Publish();
                }

                Assert.That(parentEventScope.Events.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void TestPublish_WhenMultipleNestedEventsScopeGiven_ThenEventsAreAddedToParentEventScope()
        {
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()));
            var @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(var parentEventScope = new EventsScope())
            {
                using(var childEventScope = new EventsScope())
                {
                    childEventScope.AddEvent(@event);

                    using(var nestedChildEventScope = new EventsScope())
                    {
                        nestedChildEventScope.AddEvent(@event);

                        nestedChildEventScope.Publish();
                    }

                    childEventScope.Publish();
                }

                using(var childEventScope = new EventsScope())
                {
                    childEventScope.AddEvent(@event);

                    childEventScope.Publish();
                }

                Assert.That(parentEventScope.Events.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public void TestPublish_WhenParentScopeIsDisposedBeforeChildPublish_ThenInvalidOperationExceptionIsThrown()
        {
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()));
            var @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            var parentEventScope = new EventsScope();
            var nestedChildEventScope = new EventsScope();
            nestedChildEventScope.AddEvent(@event);
            parentEventScope.Dispose();

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Parent events scope was disposed."),
                () => nestedChildEventScope.Publish());
        }

        [Test]
        public void TestPublish_WhenPublishingWithoutParentEventScope_ThenEventsAreDispatched()
        {
            bool dispatched = false;
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback(() => dispatched = true);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var eventsScope = new EventsScope();
            var @event = eventMock.Object;
            eventsScope.AddEvent(@event);

            eventsScope.Publish();

            Assert.That(dispatched, Is.True);
        }

        [Test]
        public void TestPublish_WhenPublishing_ThenEventsAreCleared()
        {
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var eventsScope = new EventsScope();
            var @event = eventMock.Object;
            eventsScope.AddEvent(@event);

            eventsScope.Publish();

            Assert.That(eventsScope.Events, Is.Empty);
        }

        [Test]
        public void TestClear_WhenClearing_ThenEventsAreEmpty()
        {
            var eventMock = new Mock<IEvent>();
            var eventsScope = new EventsScope();
            var @event = eventMock.Object;
            eventsScope.AddEvent(@event);

            eventsScope.Clear();

            Assert.That(eventsScope.Events, Is.Empty);
        }

        [Test]
        public void TestDispose_WhenDisposingCurrentScope_ThenCurrentScopeIsNull()
        {
            EventsScope eventsScope = new EventsScope();

            eventsScope.Dispose();

            Assert.That(EventManager.CurrentScope, Is.Null);
        }
    }
}