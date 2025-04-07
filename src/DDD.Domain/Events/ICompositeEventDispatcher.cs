using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Events;

public interface ICompositeEventDispatcher : IEventDispatcher
{
    void Add(IEventDispatcher eventDispatcher);
}
