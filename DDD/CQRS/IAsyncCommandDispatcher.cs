using System.Threading.Tasks;

namespace DDD.CQRS
{
    public interface IAsyncCommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}