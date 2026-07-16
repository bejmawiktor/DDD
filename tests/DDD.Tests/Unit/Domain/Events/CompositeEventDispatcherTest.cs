using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using Moq;

namespace DDD.Tests.Unit.Domain.Events;

internal class CompositeEventDispatcherTest
{
    [Test]
    public async Task TestDispatch_WhenMultipleDispatchersAdded_ThenMultipleDispatchAreExecuted()
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

        using (Assert.Multiple())
        {
            await Assert.That(firstDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(secondDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(thirdDispatchedEvent).IsSameReferenceAs(@event);
        }
    }

    [Test]
    public async Task TestDispatch_WhenNoDispatchersAdded_ThenNoDispatchAreExecuted()
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

        using (Assert.Multiple())
        {
            await Assert.That(firstDispatchedEvent).IsNull();
            await Assert.That(secondDispatchedEvent).IsNull();
            await Assert.That(thirdDispatchedEvent).IsNull();
        }
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

        using (Assert.Multiple())
        {
            await Assert.That(firstDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(secondDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(thirdDispatchedEvent).IsSameReferenceAs(@event);
        }
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

        using (Assert.Multiple())
        {
            await Assert.That(firstDispatchedEvent).IsNull();
            await Assert.That(secondDispatchedEvent).IsNull();
            await Assert.That(thirdDispatchedEvent).IsNull();
        }
    }

    [Test]
    public async Task TestDispatchUsingAddRange_WhenMultipleDispatchersAdded_ThenMultipleDispatchAreExecuted()
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

        using (Assert.Multiple())
        {
            await Assert.That(firstDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(secondDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(thirdDispatchedEvent).IsSameReferenceAs(@event);
        }
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

        using (Assert.Multiple())
        {
            await Assert.That(firstDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(secondDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(thirdDispatchedEvent).IsSameReferenceAs(@event);
        }
    }

    [Test]
    public async Task TestAdd_WhenNullDispatcherGiven_ThenArgumentNullExceptionIsThrown()
    {
        CompositeEventDispatcher compositeDispatcher = new();

        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(
            () => compositeDispatcher.Add(null!)
        );

        await Assert.That(exception!.ParamName).IsEqualTo("dispatcher");
    }

    [Test]
    public async Task TestAddRange_WhenNullDispatcherGiven_ThenArgumentNullExceptionIsThrown()
    {
        CompositeEventDispatcher compositeDispatcher = new();

        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(
            () => compositeDispatcher.AddRange(null!)
        );

        await Assert.That(exception!.ParamName).IsEqualTo("dispatchers");
    }
}
