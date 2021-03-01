namespace DDD.Application.CQRS
{
    public interface IQueryDispatcher
    {
        TResult Dispatch<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>;
    }
}