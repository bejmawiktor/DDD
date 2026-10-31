using DDD.Domain.Events;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;

namespace DDD.Tests.Unit.Domain.Events;

[NotInParallel]
public class TransactionalEventsScopeTest
{
    [After(Test)]
    public void ClearEventManager()
    {
        EventManager.CurrentScope?.Dispose();
        EventManager.Instance.Dispatcher = null;
    }

    [Test]
    public async Task TestConstructor_WhenCreating_ThenEventsScopeIsSetAsCurrentScope()
    {
        using TransactionalEventsScope transactionalEventsScope = new();

        _ = await Assert.That(EventManager.CurrentScope).IsNotNull();
    }

    [Test]
    public async Task TestConstructor_WhenOtherEventsScopeIsActive_ThenInvalidOperationExceptionIsThrown()
    {
        using EventsScope eventsScope = new();

        InvalidOperationException? exception = Assert.Throws<InvalidOperationException>(() =>
            new TransactionalEventsScope()
        );

        using (Assert.Multiple())
        {
            _ = await Assert
                .That(exception!.Message)
                .IsEqualTo("Transactional events scope must be the outermost events scope.");
            _ = await Assert.That(EventManager.CurrentScope).IsSameReferenceAs(eventsScope);
        }
    }

    [Test]
    public async Task TestDispose_WhenDisposingWithoutPublishing_ThenEventsAreNotDispatched()
    {
        bool dispatched = false;
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<EventStub>()))
            .Callback(() => dispatched = true);
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using (TransactionalEventsScope transactionalEventsScope = new())
        {
            EventManager.Instance.Notify(new EventStub());
        }

        _ = await Assert.That(dispatched).IsFalse();
    }

    [Test]
    public async Task TestPublishAsync_WhenNullDispatcherGiven_ThenArgumentNullExceptionIsThrown()
    {
        using TransactionalEventsScope transactionalEventsScope = new();

        ArgumentNullException? exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            transactionalEventsScope.PublishAsync(null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("dispatcher");
    }

    [Test]
    public async Task TestPublishAsync_WhenEventsNotified_ThenEventsAreDispatchedToGivenDispatcher()
    {
        List<EventStub> events = [];
        Mock<ITransactionalEventDispatcher> transactionalEventDispatcherMock = new();
        _ = transactionalEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
            .Callback((EventStub e) => events.Add(e))
            .Returns(Task.CompletedTask);
        EventStub firstEvent = new();
        EventStub secondEvent = new();

        using (TransactionalEventsScope transactionalEventsScope = new())
        {
            EventManager.Instance.Notify(firstEvent);
            EventManager.Instance.Notify(secondEvent);

            await transactionalEventsScope.PublishAsync(transactionalEventDispatcherMock.Object);
        }

        _ = await Assert.That(events).IsEquivalentTo([firstEvent, secondEvent]);
    }

    [Test]
    public async Task TestPublishAsync_WhenEventsNotified_ThenGlobalDispatcherIsNotUsed()
    {
        bool dispatchedByGlobalDispatcher = false;
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
            .Callback(() => dispatchedByGlobalDispatcher = true)
            .Returns(Task.CompletedTask);
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;
        Mock<ITransactionalEventDispatcher> transactionalEventDispatcherMock = new();
        _ = transactionalEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
            .Returns(Task.CompletedTask);

        using (TransactionalEventsScope transactionalEventsScope = new())
        {
            EventManager.Instance.Notify(new EventStub());

            await transactionalEventsScope.PublishAsync(transactionalEventDispatcherMock.Object);
        }

        _ = await Assert.That(dispatchedByGlobalDispatcher).IsFalse();
    }

    [Test]
    public async Task TestPublishAsync_WhenNestedEventsScopePublished_ThenEventsAreDispatchedToGivenDispatcher()
    {
        List<EventStub> events = [];
        Mock<ITransactionalEventDispatcher> transactionalEventDispatcherMock = new();
        _ = transactionalEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
            .Callback((EventStub e) => events.Add(e))
            .Returns(Task.CompletedTask);
        EventStub @event = new();

        using (TransactionalEventsScope transactionalEventsScope = new())
        {
            using (EventsScope nestedEventsScope = new())
            {
                EventManager.Instance.Notify(@event);

                await nestedEventsScope.PublishAsync();
            }

            await transactionalEventsScope.PublishAsync(transactionalEventDispatcherMock.Object);
        }

        _ = await Assert.That(events).IsEquivalentTo([@event]);
    }

    [Test]
    public async Task TestDispose_WhenDisposing_ThenCurrentScopeIsNull()
    {
        using (TransactionalEventsScope transactionalEventsScope = new())
        {
            //
        }

        _ = await Assert.That(EventManager.CurrentScope).IsNull();
    }
}
