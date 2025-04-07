using System;
using System.Threading.Tasks;
using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events;

[TestFixture]
internal class CompositeEventDispatcherTest
{
    [Test]
    public void TestDispatch_WhenMultipleDispatchersAdded_ThenMultipleDispatchAreExecuted()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    firstDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    secondDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    thirdDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        CompositeEventDispatcher compositeDispatcher = new();
        compositeDispatcher.Add(firstEventDispatcherMock.Object);
        compositeDispatcher.Add(secondEventDispatcherMock.Object);
        compositeDispatcher.Add(thirdEventDispatcherMock.Object);

        compositeDispatcher.Dispatch(@event);

        Assert.Multiple(() =>
        {
            Assert.That(firstDispatchedEvent, Is.SameAs(@event));
            Assert.That(secondDispatchedEvent, Is.SameAs(@event));
            Assert.That(thirdDispatchedEvent, Is.SameAs(@event));
        });
    }

    [Test]
    public void TestDispatch_WhenNoDispatchersAdded_ThenNoDispatchAreExecuted()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    firstDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    secondDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    thirdDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        CompositeEventDispatcher compositeDispatcher = new();

        compositeDispatcher.Dispatch(@event);

        Assert.Multiple(() =>
        {
            Assert.That(firstDispatchedEvent, Is.Null);
            Assert.That(secondDispatchedEvent, Is.Null);
            Assert.That(thirdDispatchedEvent, Is.Null);
        });
    }

    [Test]
    public async Task TestDispatchAsync_WhenMultipleDispatchersAdded_ThenMultipleDispatchAreExecuted()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Returns(
                async (IEvent dispatchedEvent) =>
                {
                    await Task.Run(() =>
                    {
                        firstDispatchedEvent = dispatchedEvent as EventStub;
                    });
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Returns(
                async (IEvent dispatchedEvent) =>
                {
                    await Task.Run(() =>
                    {
                        secondDispatchedEvent = dispatchedEvent as EventStub;
                    });
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Returns(
                async (IEvent dispatchedEvent) =>
                {
                    await Task.Run(() =>
                    {
                        thirdDispatchedEvent = dispatchedEvent as EventStub;
                    });
                }
            );
        CompositeEventDispatcher compositeDispatcher = new();
        compositeDispatcher.Add(firstEventDispatcherMock.Object);
        compositeDispatcher.Add(secondEventDispatcherMock.Object);
        compositeDispatcher.Add(thirdEventDispatcherMock.Object);

        await compositeDispatcher.DispatchAsync(@event);

        Assert.Multiple(() =>
        {
            Assert.That(firstDispatchedEvent, Is.SameAs(@event));
            Assert.That(secondDispatchedEvent, Is.SameAs(@event));
            Assert.That(thirdDispatchedEvent, Is.SameAs(@event));
        });
    }

    [Test]
    public async Task TestDispatchAsync_WhenNoDispatchersAdded_ThenNoDispatchAreExecuted()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Returns(
                async (IEvent dispatchedEvent) =>
                {
                    await Task.Run(() =>
                    {
                        firstDispatchedEvent = dispatchedEvent as EventStub;
                    });
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Returns(
                async (IEvent dispatchedEvent) =>
                {
                    await Task.Run(() =>
                    {
                        secondDispatchedEvent = dispatchedEvent as EventStub;
                    });
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Returns(
                async (IEvent dispatchedEvent) =>
                {
                    await Task.Run(() =>
                    {
                        thirdDispatchedEvent = dispatchedEvent as EventStub;
                    });
                }
            );
        CompositeEventDispatcher compositeDispatcher = new();

        await compositeDispatcher.DispatchAsync(@event);

        Assert.Multiple(() =>
        {
            Assert.That(firstDispatchedEvent, Is.Null);
            Assert.That(secondDispatchedEvent, Is.Null);
            Assert.That(thirdDispatchedEvent, Is.Null);
        });
    }

    [Test]
    public void TestDispatchUsingAddRange_WhenMultipleDispatchersAdded_ThenMultipleDispatchAreExecuted()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    firstDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    secondDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    thirdDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        CompositeEventDispatcher compositeDispatcher = new();
        compositeDispatcher.AddRange(
            [
                firstEventDispatcherMock.Object,
                secondEventDispatcherMock.Object,
                thirdEventDispatcherMock.Object,
            ]
        );

        compositeDispatcher.Dispatch(@event);

        Assert.Multiple(() =>
        {
            Assert.That(firstDispatchedEvent, Is.SameAs(@event));
            Assert.That(secondDispatchedEvent, Is.SameAs(@event));
            Assert.That(thirdDispatchedEvent, Is.SameAs(@event));
        });
    }

    [Test]
    public async Task TestDispatchAsyncUsingAddRange_WhenMultipleDispatchersAdded_ThenMultipleDispatchAreExecuted()
    {
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Returns(
                async (IEvent dispatchedEvent) =>
                {
                    await Task.Run(() =>
                    {
                        firstDispatchedEvent = dispatchedEvent as EventStub;
                    });
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Returns(
                async (IEvent dispatchedEvent) =>
                {
                    await Task.Run(() =>
                    {
                        secondDispatchedEvent = dispatchedEvent as EventStub;
                    });
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<IEvent>()))
            .Returns(
                async (IEvent dispatchedEvent) =>
                {
                    await Task.Run(() =>
                    {
                        thirdDispatchedEvent = dispatchedEvent as EventStub;
                    });
                }
            );
        CompositeEventDispatcher compositeDispatcher = new();
        compositeDispatcher.AddRange(
            [
                firstEventDispatcherMock.Object,
                secondEventDispatcherMock.Object,
                thirdEventDispatcherMock.Object,
            ]
        );

        await compositeDispatcher.DispatchAsync(@event);

        Assert.Multiple(() =>
        {
            Assert.That(firstDispatchedEvent, Is.SameAs(@event));
            Assert.That(secondDispatchedEvent, Is.SameAs(@event));
            Assert.That(thirdDispatchedEvent, Is.SameAs(@event));
        });
    }

    [Test]
    public void TestAdd_WhenNullDispatcherGiven_ThenArgumentNullExceptionIsThrown()
    {
        CompositeEventDispatcher compositeDispatcher = new();

        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("dispatcher"),
            () => compositeDispatcher.Add(null!)
        );
    }

    [Test]
    public void TestAddRange_WhenNullDispatcherGiven_ThenArgumentNullExceptionIsThrown()
    {
        CompositeEventDispatcher compositeDispatcher = new();

        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("dispatchers"),
            () => compositeDispatcher.AddRange(null!)
        );
    }
}
