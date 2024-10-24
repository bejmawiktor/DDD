namespace DDD.Domain.Persistence;

public interface IUnitOfWork
{
    ITransactionScope BeginScope();
}