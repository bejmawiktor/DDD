using DDD.Application.Persistence;
using DDD.Domain.Persistence;
using DDD.Tests.Unit.Domain.TestDoubles;

namespace DDD.Tests.Unit.Application.TestDoubles;

public class EventsUnitOfWorkFake : IAsyncEventsUnitOfWork, IEventsUnitOfWork
{
    public AsyncEventsTransactionScopeFake AsyncTransactionScope { get; } = new();
    public EventsTransactionScopeFake TransactionScope { get; } = new();

    public Task<IAsyncEventsTransactionScope> BeginTransactionAsync() =>
        Task.FromResult<IAsyncEventsTransactionScope>(this.AsyncTransactionScope);

    public IEventsTransactionScope BeginTransaction() => this.TransactionScope;
}
