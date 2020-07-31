using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.CQRS
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}
