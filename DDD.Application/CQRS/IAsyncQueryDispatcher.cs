using System.Threading.Tasks;

namespace DDD.Application.CQRS
{
    public interface IAsyncQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>;
    }
}