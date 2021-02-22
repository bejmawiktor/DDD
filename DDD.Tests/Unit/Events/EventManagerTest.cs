using DDD.Events;
using DDD.Tests.Unit.TestDoubles;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Events
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
        public void TestCreateScope_WhenScopeCreated_ThenCurrentScopeIsSet()
        {
            EventsScope eventsScope = EventManager.Instance.CreateScope();

            Assert.That(EventManager.CurrentScope, Is.SameAs(eventsScope));
        }

        [Test]
        public void TestCreateScope_WhenScopeWasAlreadyCreated_ThenInvalidOperationExceptionIsThrown()
        {
            EventsScope eventsScope = EventManager.Instance.CreateScope();

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Can't begin another scope when last one wasn't disposed."),
                () => EventManager.Instance.CreateScope());
        }

        [Test]
        public void TestCreateScope_WhenMultipleScopesAreCreatedAsynchronously_ThenInvalidOperationExceptionIsNotThrown()
        {
            bool exceptionThrown = false;

            var firstTask = new Task(() =>
            {
                try
                {
                    using(EventsScope eventsScope = EventManager.Instance.CreateScope())
                    {
                    }
                }
                catch(AggregateException)
                {
                    exceptionThrown = true;
                }
            });
            var secondTask = Task.Run(() =>
            {
                try
                {
                    using(EventsScope eventsScope = EventManager.Instance.CreateScope())
                    {
                        firstTask.Start();
                        firstTask.Wait();
                    }
                }
                catch(AggregateException)
                {
                    exceptionThrown = true;
                }
            });

            secondTask.Wait();

            Assert.That(exceptionThrown, Is.False);
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

            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
            {
                EventManager.Instance.Notify(eventMock.Object);
            }

            Assert.That(dispatched, Is.False);
        }

        [Test]
        public void TestNotify_WhenScopeWasCreated_ThenEventsAreAddedToScopeEvents()
        {
            bool dispatched;
            var eventMock = new Mock<IEvent>();
            var @event = eventMock.Object;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback(() => dispatched = true);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
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