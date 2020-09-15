using System.Threading.Tasks;

namespace DDD.CQRS
{
    public interface IAsyncCommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}