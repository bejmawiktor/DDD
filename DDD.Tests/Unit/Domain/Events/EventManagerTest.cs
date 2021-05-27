using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events
{
    [TestFixture]
    public class EventManagerTest
    {
        [TearDown]
        public void ClearEventManager()
        {
            EventManager.CurrentScope = null;
            EventManager.Instance.EventDispatcher = null;
        }

        [Test]
        public void TestEventManager_WhenNoEventScopeCreated_ThenEventsAreDispatchedImmediately()
        {
            bool dispatched = false;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<EventStub>()))
                .Callback(() => dispatched = true);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            EventManager.Instance.Notify(new EventStub());

            Assert.That(dispatched, Is.True);
        }

        [Test]
        public void TestNotify_WhenScopeWasCreated_ThenEventsArentDispatched()
        {
            bool dispatched = false;
            var eventMock = new Mock<IEvent>();
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback(() => dispatched = true);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(EventsScope eventsScope = new EventsScope())
            {
                EventManager.Instance.Notify(eventMock.Object);
            }

            Assert.That(dispatched, Is.False);
        }

        [Test]
        public void TestNotify_WhenScopeWasCreated_ThenEventsAreAddedToScopeEvents()
        {
            var eventMock = new Mock<IEvent>();
            var @event = eventMock.Object;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(EventsScope eventsScope = new EventsScope())
            {
                EventManager.Instance.Notify(eventMock.Object);

                Assert.That(eventsScope.Events.Contains(@event));
            }
        }

        [Test]
        public void TestNotify_WhenScopeWasntCreated_ThenEventsAreDispatched()
        {
            bool dispatched = false;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<EventStub>()))
                .Callback(() => dispatched = true);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            EventManager.Instance.Notify(new EventStub());

            Assert.That(dispatched, Is.True);
        }
    }
}