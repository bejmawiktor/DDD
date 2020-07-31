namespace DDD.CQRS
{
    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(TCommand query)
            where TCommand : ICommand;
    }
}