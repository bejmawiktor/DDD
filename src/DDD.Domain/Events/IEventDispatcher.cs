using System.Threading.Tasks;
using DDD.Domain.Common;

namespace DDD.Domain.Events
{
    public interface IEventDispatcher : IDispatcher<IEvent> { }
}
