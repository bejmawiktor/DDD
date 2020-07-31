using System;

namespace DDD.CQRS
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private IDependencyResolver DependencyResolver { get; }

        public QueryDispatcher(IDependencyResolver dependencyResolver)
        {
            this.DependencyResolver = dependencyResolver
                ?? throw new ArgumentNullException(nameof(dependencyResolver));
        }

        public TResult Dispatch<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            if(query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var handler = this.DependencyResolver.Resolve<IQueryHandler<TQuery, TResult>>();

            if(handler == null)
            {
                throw new QueryHandlerNotFoundException();
            }

            return handler.Handle(query);
        }
    }
}