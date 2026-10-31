using DDD.Domain.Events;
using DDD.Domain.Persistence;
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
    public async Task TestConstructor_WhenNullAsyncTransactionGiven_ThenArgumentNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            new TransactionalEventsScope((IAsyncEventsTransactionScope)null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("transactionScope");
    }

    [Test]
    public async Task TestConstructor_WhenNullSyncTransactionGiven_ThenArgumentNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            new TransactionalEventsScope((IEventsTransactionScope)null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("transactionScope");
    }

    [Test]
    public async Task TestConstructor_WhenAsyncTransactionGiven_ThenScopeIsAttachedToTransaction()
    {
        AsyncEventsTransactionScopeFake transactionScope = new();

        using TransactionalEventsScope transactionalEventsScope = new(transactionScope);

        using (Assert.Multiple())
        {
            _ = await Assert
                .That(transactionScope.AttachedEventsScope)
                .IsSameReferenceAs(transactionalEventsScope);
            _ = await Assert.That(EventManager.CurrentScope).IsNotNull();
        }
    }

    [Test]
    public async Task TestConstructor_WhenSyncTransactionGiven_ThenScopeIsAttachedToTransaction()
    {
        EventsTransactionScopeFake transactionScope = new();

        using TransactionalEventsScope transactionalEventsScope = new(transactionScope);

        _ = await Assert
            .That(transactionScope.AttachedEventsScope)
            .IsSameReferenceAs(transactionalEventsScope);
    }

    [Test]
    public async Task TestConstructor_WhenOtherEventsScopeIsActive_ThenInvalidOperationExceptionIsThrown()
    {
        AsyncEventsTransactionScopeFake transactionScope = new();
        using EventsScope eventsScope = new();

        InvalidOperationException? exception = Assert.Throws<InvalidOperationException>(() =>
            new TransactionalEventsScope(transactionScope)
        );

        using (Assert.Multiple())
        {
            _ = await Assert
                .That(exception!.Message)
                .IsEqualTo("Transactional events scope must be the outermost events scope.");
            _ = await Assert.That(transactionScope.AttachedEventsScope).IsNull();
            _ = await Assert.That(EventManager.CurrentScope).IsSameReferenceAs(eventsScope);
        }
    }

    [Test]
    public async Task TestConstructor_WhenTransactionRefusesAttachment_ThenNoScopeIsLeftActive()
    {
        Mock<IAsyncEventsTransactionScope> transactionScopeMock = new();
        _ = transactionScopeMock
            .Setup(t => t.Attach(It.IsAny<TransactionalEventsScope>()))
            .Throws(new InvalidOperationException("Transaction refused the events scope."));

        _ = Assert.Throws<InvalidOperationException>(() =>
            new TransactionalEventsScope(transactionScopeMock.Object)
        );

        _ = await Assert.That(EventManager.CurrentScope).IsNull();
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
        Mock<IAsyncTransactionalEventDispatcher> transactionalEventDispatcherMock = new();
        _ = transactionalEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
            .Callback(() => dispatched = true)
            .Returns(Task.CompletedTask);

        using (
            TransactionalEventsScope transactionalEventsScope = new(
                new AsyncEventsTransactionScopeFake(transactionalEventDispatcherMock.Object)
            )
        )
        {
            EventManager.Instance.Notify(new EventStub());
        }

        _ = await Assert.That(dispatched).IsFalse();
    }

    [Test]
    public async Task TestDispose_WhenDisposing_ThenCurrentScopeIsNull()
    {
        using (
            TransactionalEventsScope transactionalEventsScope = new(
                new AsyncEventsTransactionScopeFake()
            )
        )
        {
            //
        }

        _ = await Assert.That(EventManager.CurrentScope).IsNull();
    }

    [Test]
    public async Task TestAttachDispatcher_WhenNullAsyncDispatcherGiven_ThenArgumentNullExceptionIsThrown()
    {
        using TransactionalEventsScope transactionalEventsScope = new(
            new AsyncEventsTransactionScopeFake()
        );

        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            transactionalEventsScope.AttachDispatcher((IAsyncTransactionalEventDispatcher)null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("dispatcher");
    }

    [Test]
    public async Task TestAttachDispatcher_WhenNullSyncDispatcherGiven_ThenArgumentNullExceptionIsThrown()
    {
        using TransactionalEventsScope transactionalEventsScope = new(
            new EventsTransactionScopeFake()
        );

        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            transactionalEventsScope.AttachDispatcher((ISyncTransactionalEventDispatcher)null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("dispatcher");
    }

    [Test]
    public async Task TestAttachDispatcher_WhenDispatcherAlreadyAttached_ThenInvalidOperationExceptionIsThrown()
    {
        using TransactionalEventsScope transactionalEventsScope = new(
            new AsyncEventsTransactionScopeFake(
                new Mock<IAsyncTransactionalEventDispatcher>().Object
            )
        );

        InvalidOperationException? exception = Assert.Throws<InvalidOperationException>(() =>
            transactionalEventsScope.AttachDispatcher(
                new Mock<ISyncTransactionalEventDispatcher>().Object
            )
        );

        _ = await Assert
            .That(exception!.Message)
            .IsEqualTo("Transactional events scope is already attached to a transaction.");
    }

    [Test]
    public async Task TestPublishAsync_WhenTransactionAttachedNoDispatcher_ThenInvalidOperationExceptionIsThrown()
    {
        using TransactionalEventsScope transactionalEventsScope = new(
            new AsyncEventsTransactionScopeFake()
        );

        InvalidOperationException? exception = await Assert.ThrowsAsync<InvalidOperationException>(
            transactionalEventsScope.PublishAsync
        );

        _ = await Assert
            .That(exception!.Message)
            .IsEqualTo(
                "Transactional events scope is not attached to an asynchronous transaction."
            );
    }

    [Test]
    public async Task TestPublishAsync_WhenSyncTransactionGiven_ThenInvalidOperationExceptionIsThrown()
    {
        using TransactionalEventsScope transactionalEventsScope = new(
            new EventsTransactionScopeFake(new Mock<ISyncTransactionalEventDispatcher>().Object)
        );

        InvalidOperationException? exception = await Assert.ThrowsAsync<InvalidOperationException>(
            transactionalEventsScope.PublishAsync
        );

        _ = await Assert
            .That(exception!.Message)
            .IsEqualTo(
                "Transactional events scope is not attached to an asynchronous transaction."
            );
    }

    [Test]
    public async Task TestPublishAsync_WhenEventsNotified_ThenEventsAreDispatchedToTransactionDispatcher()
    {
        List<EventStub> events = [];
        Mock<IAsyncTransactionalEventDispatcher> transactionalEventDispatcherMock = new();
        _ = transactionalEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
            .Callback((EventStub e) => events.Add(e))
            .Returns(Task.CompletedTask);
        EventStub firstEvent = new();
        EventStub secondEvent = new();

        using (
            TransactionalEventsScope transactionalEventsScope = new(
                new AsyncEventsTransactionScopeFake(transactionalEventDispatcherMock.Object)
            )
        )
        {
            EventManager.Instance.Notify(firstEvent);
            EventManager.Instance.Notify(secondEvent);

            await transactionalEventsScope.PublishAsync();
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
        Mock<IAsyncTransactionalEventDispatcher> transactionalEventDispatcherMock = new();
        _ = transactionalEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
            .Returns(Task.CompletedTask);

        using (
            TransactionalEventsScope transactionalEventsScope = new(
                new AsyncEventsTransactionScopeFake(transactionalEventDispatcherMock.Object)
            )
        )
        {
            EventManager.Instance.Notify(new EventStub());

            await transactionalEventsScope.PublishAsync();
        }

        _ = await Assert.That(dispatchedByGlobalDispatcher).IsFalse();
    }

    [Test]
    public async Task TestPublishAsync_WhenNestedEventsScopePublished_ThenEventsAreDispatchedToTransactionDispatcher()
    {
        List<EventStub> events = [];
        Mock<IAsyncTransactionalEventDispatcher> transactionalEventDispatcherMock = new();
        _ = transactionalEventDispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<EventStub>()))
            .Callback((EventStub e) => events.Add(e))
            .Returns(Task.CompletedTask);
        EventStub @event = new();

        using (
            TransactionalEventsScope transactionalEventsScope = new(
                new AsyncEventsTransactionScopeFake(transactionalEventDispatcherMock.Object)
            )
        )
        {
            using (EventsScope nestedEventsScope = new())
            {
                EventManager.Instance.Notify(@event);

                await nestedEventsScope.PublishAsync();
            }

            await transactionalEventsScope.PublishAsync();
        }

        _ = await Assert.That(events).IsEquivalentTo([@event]);
    }

    [Test]
    public async Task TestPublish_WhenAsyncTransactionGiven_ThenInvalidOperationExceptionIsThrown()
    {
        using TransactionalEventsScope transactionalEventsScope = new(
            new AsyncEventsTransactionScopeFake(
                new Mock<IAsyncTransactionalEventDispatcher>().Object
            )
        );

        InvalidOperationException? exception = Assert.Throws<InvalidOperationException>(
            transactionalEventsScope.Publish
        );

        _ = await Assert
            .That(exception!.Message)
            .IsEqualTo("Transactional events scope is not attached to a synchronous transaction.");
    }

    [Test]
    public async Task TestPublish_WhenEventsNotified_ThenEventsAreDispatchedToTransactionDispatcher()
    {
        List<EventStub> events = [];
        Mock<ISyncTransactionalEventDispatcher> transactionalEventDispatcherMock = new();
        _ = transactionalEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<EventStub>()))
            .Callback((EventStub e) => events.Add(e));
        EventStub firstEvent = new();
        EventStub secondEvent = new();

        using (
            TransactionalEventsScope transactionalEventsScope = new(
                new EventsTransactionScopeFake(transactionalEventDispatcherMock.Object)
            )
        )
        {
            EventManager.Instance.Notify(firstEvent);
            EventManager.Instance.Notify(secondEvent);

            transactionalEventsScope.Publish();
        }

        _ = await Assert.That(events).IsEquivalentTo([firstEvent, secondEvent]);
    }

    [Test]
    public async Task TestPublish_WhenEventsNotified_ThenGlobalDispatcherIsNotUsed()
    {
        bool dispatchedByGlobalDispatcher = false;
        Mock<IEventDispatcher> eventDispatcherMock = new();
        _ = eventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<EventStub>()))
            .Callback(() => dispatchedByGlobalDispatcher = true);
        EventManager.Instance.Dispatcher = eventDispatcherMock.Object;

        using (
            TransactionalEventsScope transactionalEventsScope = new(
                new EventsTransactionScopeFake(new Mock<ISyncTransactionalEventDispatcher>().Object)
            )
        )
        {
            EventManager.Instance.Notify(new EventStub());

            transactionalEventsScope.Publish();
        }

        _ = await Assert.That(dispatchedByGlobalDispatcher).IsFalse();
    }
}
