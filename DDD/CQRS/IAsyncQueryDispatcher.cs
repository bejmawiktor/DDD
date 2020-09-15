using System.Threading.Tasks;

namespace DDD.CQRS
{
    public interface IAsyncQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>;
    }
}