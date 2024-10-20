using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.Common;

internal interface IScopeHandler<TScope, in TItem, TScopeHandler>
    where TScope : Scope<TItem, TScope, TScopeHandler>
    where TScopeHandler : IScopeHandler<TScope, TItem, TScopeHandler>, new()
{
    static abstract TScope? CurrentScope { get; internal set; }

    static abstract TScopeHandler Instance { get; }

    void Notify(TItem item)
    {
        if (TScopeHandler.CurrentScope is null)
        {
            if (this.Dispatcher is null)
            {
                throw new InvalidOperationException("Dispatcher is uninitialized.");
            }

            this.Dispatcher.Dispatch(item);
        }
        else
        {
            TScopeHandler.CurrentScope.Add(item);
        }
    }

    Task NotifyAsync(TItem item)
    {
        if (TScopeHandler.CurrentScope is null)
        {
            return this.Dispatcher is null
                ? throw new InvalidOperationException("Dispatcher is uninitialized.")
                : this.Dispatcher.DispatchAsync(item);
        }

        TScopeHandler.CurrentScope.Add(item);

        return Task.CompletedTask;
    }

    public IDispatcher? Dispatcher { get; set; }
}
