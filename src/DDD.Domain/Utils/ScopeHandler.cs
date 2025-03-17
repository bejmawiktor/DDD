using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.Utils;

public abstract class ScopeHandler<TScope, TItem, TScopeHandler>
    : IScopeContainer<TScope, TScopeHandler>
    where TScope : Scope<TItem, TScope, TScopeHandler>
    where TScopeHandler : ScopeHandler<TScope, TItem, TScopeHandler>, new()
    where TItem : notnull
{
    private static readonly Lazy<TScopeHandler> instance = new(() => new TScopeHandler());

    private static readonly AsyncLocal<TScope?> localEventsScope = new();

    public static TScope? CurrentScope
    {
        get => ScopeHandler<TScope, TItem, TScopeHandler>.localEventsScope.Value;
        internal set => ScopeHandler<TScope, TItem, TScopeHandler>.localEventsScope.Value = value;
    }

    static TScope? IScopeContainer<TScope, TScopeHandler>.CurrentScope
    {
        get => ScopeHandler<TScope, TItem, TScopeHandler>.CurrentScope;
        set => ScopeHandler<TScope, TItem, TScopeHandler>.CurrentScope = value;
    }

    public static TScopeHandler Instance => instance.Value;

    protected internal virtual void Handle<TSpecificItem>(TSpecificItem item)
        where TSpecificItem : TItem
    {
        if (ScopeHandler<TScope, TItem, TScopeHandler>.CurrentScope is null)
        {
            if (this.Dispatcher is null)
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

    protected internal virtual Task HandleAsync<TSpecificItem>(TSpecificItem item)
        where TSpecificItem : TItem
    {
        if (ScopeHandler<TScope, TItem, TScopeHandler>.CurrentScope is null)
        {
            return this.Dispatcher is null
                ? throw new InvalidOperationException("Dispatcher is uninitialized.")
                : this.Dispatcher.DispatchAsync(item);
        }

        ScopeHandler<TScope, TItem, TScopeHandler>.CurrentScope.Add(item);

        return Task.CompletedTask;
    }

    public abstract IDispatcher<TItem>? Dispatcher { get; set; }
}
