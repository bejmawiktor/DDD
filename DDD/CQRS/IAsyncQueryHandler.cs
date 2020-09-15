using System.Threading.Tasks;

namespace DDD.CQRS
{
    public interface IAsyncQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}