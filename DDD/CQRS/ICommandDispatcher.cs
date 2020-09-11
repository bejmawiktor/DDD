namespace DDD.CQRS
{
    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}