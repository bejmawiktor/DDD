﻿using System;
using System.Threading.Tasks;

namespace DDD.Application.CQRS
{
    public class CommandDispatcher : ICommandDispatcher, IAsyncCommandDispatcher
    {
        private IDependencyResolver DependencyResolver { get; }

        public CommandDispatcher(IDependencyResolver dependencyResolver)
        {
            this.DependencyResolver = dependencyResolver
                ?? throw new ArgumentNullException(nameof(dependencyResolver));
        }

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            ArgumentNullException.ThrowIfNull(command);

            ICommandHandler<TCommand> handler = this.DependencyResolver.Resolve<ICommandHandler<TCommand>>();

            if(handler == null)
            {
                throw new CommandHandlerNotFoundException();
            }

            handler.Handle(command);
        }

        public Task DispatchAsync<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            ArgumentNullException.ThrowIfNull(command);

            IAsyncCommandHandler<TCommand> handler = this.DependencyResolver.Resolve<IAsyncCommandHandler<TCommand>>();

            if(handler == null)
            {
                throw new CommandHandlerNotFoundException();
            }

            return handler.HandleAsync(command);
        }
    }
}