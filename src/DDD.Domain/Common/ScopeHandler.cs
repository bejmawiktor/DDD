using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.Common;

public abstract class ScopeHandler<TScope, TItem, TScopeHandler>
    where TScope : Scope<TItem, TScope, TScopeHandler>
    where TScopeHandler : ScopeHandler<TScope, TItem, TScopeHandler>, new()
{
    private static readonly Lazy<TScopeHandler> instance = new(() => new TScopeHandler());

    private static readonly AsyncLocal<TScope?> localEventsScope = new();

    public static TScope? CurrentScope
    {
        get => ScopeHandler<TScope, TItem, TScopeHandler>.localEventsScope.Value;
        internal set => ScopeHandler<TScope, TItem, TScopeHandler>.localEventsScope.Value = value;
    }

    public static TScopeHandler Instance => instance.Value;

    public void Notify(TItem item)
    {
        if(ScopeHandler<TScope, TItem, TScopeHandler>.CurrentScope is null)
        {
            if(this.Dispatcher is null)
            {
                throw new InvalidOperationException("Dispatcher is uninitialized.");
            }

            this.Dispatcher.Dispatch(item);
        }
        else
        {
            ScopeHandler<TScope, TItem, TScopeHandler>.CurrentScope.Add(item);
        }
    }

    public Task NotifyAsync(TItem item)
    {
        if(ScopeHandler<TScope, TItem, TScopeHandler>.CurrentScope is null)
        {
            return this.Dispatcher is null
                ? throw new InvalidOperationException("Dispatcher is uninitialized.")
                : this.Dispatcher.DispatchAsync(item);
        }

        ScopeHandler<TScope, TItem, TScopeHandler>.CurrentScope.Add(item);

        return Task.CompletedTask;
    }

    public abstract IDispatcher? Dispatcher { get; set; }
}