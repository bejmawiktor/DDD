using System.Threading.Tasks;

namespace DDD.Application.CQRS
{
    public interface IAsyncCommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}