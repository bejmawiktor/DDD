using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

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
            EventsScope eventScope = new();

            Assert.That(eventScope.Events, Is.Empty);
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
            using(EventsScope eventScope = new())
            {
                Assert.Throws(
                    Is.InstanceOf<ArgumentNullException>()
                        .And.Property(nameof(ArgumentNullException.ParamName))
                        .EqualTo("@event"),
                    () => eventScope.Add((IEvent?)null!));
            }
        }

        [Test]
        public void TestAddEvent_WhenEventGiven_ThenEventIsAdded()
        {
            Mock<IEvent> eventMock = new();
            IEvent @event = eventMock.Object;
            EventsScope? eventsScope = null;

            using(eventsScope = new())
            {
                eventsScope.Add(@event);

                Assert.That(eventsScope.Events.Contains(@event));
            }
        }

        [Test]
        public void TestPublish_WhenPublishingWithParentEventScope_ThenEventsAreAddedToParentEventScope()
        {
            Mock<IEvent> eventMock = new();
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()));
            IEvent @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(EventsScope parentEventScope = new())
            {
                using(EventsScope childEventScope = new())
                {
                    childEventScope.Add(@event);

                    childEventScope.Publish();
                }

                Assert.That(parentEventScope.Events.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void TestPublish_WhenDispatcherIsUninitialized_ThenInvalidOperetionExceptionIsThrown()
        {
            Mock<IEvent> eventMock = new();
            IEvent @event = eventMock.Object;

            using(EventsScope eventScope = new())
            {
                eventScope.Add(@event);

                Assert.Throws(
                    Is.InstanceOf<InvalidOperationException>()
                        .And.Message
                        .EqualTo("Event dispatcher is uninitialized."),
                    () => eventScope.Publish());
            }
        }

        [Test]
        public void TestPublish_WhenMultipleNestedEventsScopesGiven_ThenEventsAreAddedToParentEventScope()
        {
            Mock<IEvent> eventMock = new();
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()));
            IEvent @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(EventsScope parentEventScope = new())
            {
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

                Assert.That(parentEventScope.Events.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public void TestPublish_WhenPublishingWithoutParentEventScope_ThenEventsAreDispatched()
        {
            bool dispatched = false;
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<EventStub>()))
                .Callback(() => dispatched = true);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
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
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            IEvent @event = eventMock.Object;
            EventsScope? eventsScope = null;

            using(eventsScope = new EventsScope())
            {
                eventsScope.Add(@event);

                eventsScope.Publish();
            }

            Assert.That(eventsScope.Events, Is.Empty);
        }

        [Test]
        public async Task TestPublishAsync_WhenPublishingWithParentEventScope_ThenEventsAreAddedToParentEventScope()
        {
            Mock<IEvent> eventMock = new();
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()));
            IEvent @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(EventsScope parentEventScope = new())
            {
                using(EventsScope childEventScope = new())
                {
                    childEventScope.Add(@event);

                    await childEventScope.PublishAsync();
                }

                Assert.That(parentEventScope.Events.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void TestPublishAsync_WhenDispatcherIsUninitialized_ThenInvalidOperetionExceptionIsThrown()
        {
            Mock<IEvent> eventMock = new();
            IEvent @event = eventMock.Object;

            using(EventsScope eventScope = new())
            {
                eventScope.Add(@event);

                Assert.ThrowsAsync(
                    Is.InstanceOf<InvalidOperationException>()
                        .And.Message
                        .EqualTo("Event dispatcher is uninitialized."),
                    async () => await eventScope.PublishAsync());
            }
        }

        [Test]
        public async Task TestPublishAsync_WhenMultipleNestedEventsScopesGiven_ThenEventsAreAddedToParentEventScope()
        {
            Mock<IEvent> eventMock = new();
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()));
            IEvent @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(EventsScope parentEventScope = new())
            {
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

                Assert.That(parentEventScope.Events.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public async Task TestPublishAsync_WhenPublishingWithoutParentEventScope_ThenEventsAreDispatched()
        {
            bool dispatched = false;
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
                .Callback(() => dispatched = true);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
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
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            IEvent @event = eventMock.Object;
            EventsScope? eventsScope;

            using(eventsScope = new EventsScope())
            {
                eventsScope.Add(@event);

                await eventsScope.PublishAsync();
            }

            Assert.That(eventsScope.Events, Is.Empty);
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

            Assert.That(eventsScope.Events, Is.Empty);
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
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()));
            IEvent @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(EventsScope parentEventScope = new())
            {
                using(EventsScope nestedChildEventScope = new())
                {
                    nestedChildEventScope.Add(@event);

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
            Mock<IEvent> eventMock = new();
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()));
            IEvent @event = eventMock.Object;
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            EventsScope? parentEventScope = null;

            try
            {
                using(parentEventScope = new EventsScope())
                {
                    using(EventsScope nestedChildEventScope = new())
                    {
                        nestedChildEventScope.Add(@event);

                        parentEventScope.Dispose();
                    }
                }
            }
            catch(InvalidOperationException)
            {
            }

            Assert.That(parentEventScope?.Events, Is.Empty);
        }
    }
}