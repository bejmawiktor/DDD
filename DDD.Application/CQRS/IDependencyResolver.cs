namespace DDD.Application.CQRS
{
    public interface IDependencyResolver
    {
        TResult Resolve<TResult>();
    }
}