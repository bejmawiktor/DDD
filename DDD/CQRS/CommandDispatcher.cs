﻿using System;

namespace DDD.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
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
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handler = this.DependencyResolver.Resolve<ICommandHandler<TCommand>>();

            if(handler == null)
            {
                throw new CommandHandlerNotFoundException();
            }

            handler.Handle(command);
        }
    }
}