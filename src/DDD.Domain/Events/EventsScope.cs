using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Domain.Common;

namespace DDD.Domain.Events
{
    public sealed class EventsScope : Scope<IEvent, EventsScope, EventManager> { }
}
