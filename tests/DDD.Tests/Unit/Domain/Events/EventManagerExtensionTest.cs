using System.Threading.Tasks;
using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;
using Utils.Disposable;

namespace DDD.Tests.Unit.Domain.Events;

[TestFixture]
internal class EventManagerExtensionTest
{
    [TearDown]
    public void ClearEventManager()
    {
        DDD.Domain.Events.EventManager.Instance.Dispatcher = null;
    }

    [Test]
    public void TestUseCompositeDispatcher_WhenNotUsedPreviously_ThenCompositeDispatcherIsSet()
    {
        EventManager.Instance.UseCompositeDispatcher();

        Assert.Multiple(() =>
        {
            Assert.That(EventManager.Instance.Dispatcher, Is.Not.Null);
            Assert.That(EventManager.Instance.Dispatcher, Is.TypeOf<CompositeEventDispatcher>());
        });
    }

    [Test]
    public void TestUseCompositeDispatcher_WhenUsedPreviously_ThenNewCompositeDispatcherIsNotSet()
    {
        EventManager.Instance.UseCompositeDispatcher();
        IDispatcher<IEvent>? eventDispatcher = EventManager.Instance.Dispatcher;

        EventManager.Instance.UseCompositeDispatcher();

        Assert.Multiple(() =>
        {
            Assert.That(EventManager.Instance.Dispatcher, Is.SameAs(eventDispatcher));
        });
    }

    [Test]
    public void TestUseCompositeDispatcher_WhenConfigurationGiven_ThenDispatchersAreSetToEventManagerDispatcher()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new Mock<IEventDispatcher>();
        firstEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    firstDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new Mock<IEventDispatcher>();
        secondEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    secondDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new Mock<IEventDispatcher>();
        thirdEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    thirdDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configuration
                .WithDispatcher(firstEventDispatcherMock.Object)
                .WithDispatcher(secondEventDispatcherMock.Object)
                .WithDispatcher(thirdEventDispatcherMock.Object)
        );

        EventManager.Instance.Notify(@event);

        Assert.Multiple(() =>
        {
            Assert.That(firstDispatchedEvent, Is.SameAs(@event));
            Assert.That(secondDispatchedEvent, Is.SameAs(@event));
            Assert.That(thirdDispatchedEvent, Is.SameAs(@event));
        });
    }

    [Test]
    public void TestUseCompositeDispatcher_WhenNextConfigurationGiven_ThenDispatchersAreAddedToEventManagerDispatcher()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub? fourthDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new Mock<IEventDispatcher>();
        firstEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    firstDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new Mock<IEventDispatcher>();
        secondEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    secondDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new Mock<IEventDispatcher>();
        thirdEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    thirdDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> fourthEventDispatcherMock = new Mock<IEventDispatcher>();
        fourthEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    fourthDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configuration
                .WithDispatcher(firstEventDispatcherMock.Object)
                .WithDispatcher(secondEventDispatcherMock.Object)
        );
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configuration
                .WithDispatcher(thirdEventDispatcherMock.Object)
                .WithDispatcher(fourthEventDispatcherMock.Object)
        );

        EventManager.Instance.Notify(@event);

        Assert.Multiple(() =>
        {
            Assert.That(firstDispatchedEvent, Is.SameAs(@event));
            Assert.That(secondDispatchedEvent, Is.SameAs(@event));
            Assert.That(thirdDispatchedEvent, Is.SameAs(@event));
            Assert.That(fourthDispatchedEvent, Is.SameAs(@event));
        });
    }
}
