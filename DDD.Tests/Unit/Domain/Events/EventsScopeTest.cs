using DDD.Domain.Events;
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
            using(var eventScope = new EventsScope())
            {
                Assert.Throws(
                    Is.InstanceOf<ArgumentNullException>()
                        .And.Property(nameof(ArgumentNullException.ParamName))
                        .EqualTo("event"),
                    () => eventScope.AddEvent((IEvent)null));
            }
        }

        [Test]
        public void TestAddEvent_WhenEventGiven_ThenEventIsAdded()
        {
            var eventMock = new Mock<IEvent>();
            var @event = eventMock.Object;
            EventsScope eventsScope = null;

            using(eventsScope = new EventsScope())
            {
                eventsScope.AddEvent(@event);

                Assert.That(eventsScope.Events.Contains(@event));
            }
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
        public void TestPublish_WhenMultipleNestedEventsScopesGiven_ThenEventsAreAddedToParentEventScope()
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
        public void TestPublish_WhenPublishingWithoutParentEventScope_ThenEventsAreDispatched()
        {
            bool dispatched = false;
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback(() => dispatched = true);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var @event = eventMock.Object;
            EventsScope eventsScope = null;

            using(eventsScope = new EventsScope())
            {
                eventsScope.AddEvent(@event);

                eventsScope.Publish();
            }

            Assert.That(dispatched, Is.True);
        }

        [Test]
        public void TestPublish_WhenPublishing_ThenEventsAreCleared()
        {
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var @event = eventMock.Object;
            EventsScope eventsScope = null;

            using(eventsScope = new EventsScope())
            {
                eventsScope.AddEvent(@event);

                eventsScope.Publish();
            }

            Assert.That(eventsScope.Events, Is.Empty);
        }

        [Test]
        public void TestClear_WhenClearing_ThenEventsAreEmpty()
        {
            var eventMock = new Mock<IEvent>();
            var @event = eventMock.Object;
            EventsScope eventsScope = null;

            using(eventsScope = new EventsScope())
            {
                eventsScope.AddEvent(@event);

                eventsScope.Clear();
            }

            Assert.That(eventsScope.Events, Is.Empty);
        }

        [Test]
        public void TestDispose_WhenDisposingCurrentScope_ThenCurrentScopeIsNull()
        {
            using(var eventsScope = new EventsScope())
            {
            }

            Assert.That(EventManager.CurrentScope, Is.Null);
        }

        [Test]
        public void TestDispose_WhenParentScopeIsDisposedBeforeChildIsDisposed_ThenInvalidOperationExceptionIsThrown()
        {
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()));
            var @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(var parentEventScope = new EventsScope())
            {
                using(var nestedChildEventScope = new EventsScope())
                {
                    nestedChildEventScope.AddEvent(@event);

                    Assert.Throws(
                        Is.InstanceOf<InvalidOperationException>()
                            .And.Message
                            .EqualTo("EventsScope nested incorrectly."),
                        () => parentEventScope.Dispose());
                }
            }
        }

        [Test]
        public void TestDispose_WhenParentScopeIsDisposedBeforeChildIsDisposed_ThenEventsAreCleared()
        {
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()));
            var @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            EventsScope parentEventScope = null;

            try
            {
                using(parentEventScope = new EventsScope())
                {
                    using(var nestedChildEventScope = new EventsScope())
                    {
                        nestedChildEventScope.AddEvent(@event);

                        parentEventScope.Dispose();
                    }
                }
            }
            catch(InvalidOperationException)
            {
            }

            Assert.That(parentEventScope.Events, Is.Empty);
        }
    }
}