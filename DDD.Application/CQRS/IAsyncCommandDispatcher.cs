using System.Threading.Tasks;

namespace DDD.Application.CQRS
{
    public interface IAsyncCommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}