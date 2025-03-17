using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Domain.Utils;

public abstract class Scope<TItem, TScope, TScopeHandler> : DisposableScope<TScope, TScopeHandler>
    where TScope : Scope<TItem, TScope, TScopeHandler>
    where TScopeHandler : ScopeHandler<TScope, TItem, TScopeHandler>, new()
    where TItem : notnull
{
    protected internal List<TItem> Items { get; set; }

    protected Scope()
        : base()
    {
        this.Items = [];
    }

    public void Add(TItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        this.Items.Add(item);
    }

    public override void Clear() => this.Items.Clear();

    protected internal void Apply()
    {
        if (this.ParentScope is null)
        {
            foreach (TItem item in this.Items)
            {
                if (ScopeHandler<TScope, TItem, TScopeHandler>.Instance.Dispatcher is null)
                {
                    throw new InvalidOperationException("Dispatcher is uninitialized.");
                }

                ScopeHandler<TScope, TItem, TScopeHandler>.Instance.Dispatcher.Dispatch(
                    (dynamic)item
                );
            }
        }
        else
        {
            this.ParentScope.AddRange(this.Items);
        }

        this.Clear();
    }

    private void AddRange(List<TItem> items) => this.Items.AddRange(items);

    protected internal Task ApplyAsync()
    {
        List<Task> tasks = [];

        if (this.ParentScope is null)
        {
            foreach (TItem item in this.Items)
            {
                if (ScopeHandler<TScope, TItem, TScopeHandler>.Instance.Dispatcher is null)
                {
                    throw new InvalidOperationException("Dispatcher is uninitialized.");
                }

                tasks.Add(
                    ScopeHandler<TScope, TItem, TScopeHandler>.Instance.Dispatcher.DispatchAsync(
                        (dynamic)item
                    )
                );
            }
        }
        else
        {
            this.ParentScope.AddRange(this.Items);
        }

        this.Clear();

        return Task.WhenAll(tasks);
    }
}
