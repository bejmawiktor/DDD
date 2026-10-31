using DDD.Tests.Unit.Application.TestDoubles;
using Utils.Persistence;

namespace DDD.Tests.Unit.Application.Persistence;

public class EventsUnitOfWorkTest
{
    [Test]
    public async Task TestBeginTransactionAsync_WhenCalledAsAsyncUnitOfWork_ThenEventsTransactionIsReturned()
    {
        EventsUnitOfWorkFake eventsUnitOfWork = new();
        IAsyncUnitOfWork unitOfWork = eventsUnitOfWork;

        IAsyncTransactionScope transactionScope = await unitOfWork.BeginTransactionAsync();

        _ = await Assert
            .That(transactionScope)
            .IsSameReferenceAs(eventsUnitOfWork.AsyncTransactionScope);
    }

    [Test]
    public async Task TestBeginTransaction_WhenCalledAsUnitOfWork_ThenEventsTransactionIsReturned()
    {
        EventsUnitOfWorkFake eventsUnitOfWork = new();
        IUnitOfWork unitOfWork = eventsUnitOfWork;

        ITransactionScope transactionScope = unitOfWork.BeginTransaction();

        _ = await Assert
            .That(transactionScope)
            .IsSameReferenceAs(eventsUnitOfWork.TransactionScope);
    }
}
