namespace DDD.CQRS
{
    public interface IDependencyResolver
    {
        TResult Resolve<TResult>();
    }
}