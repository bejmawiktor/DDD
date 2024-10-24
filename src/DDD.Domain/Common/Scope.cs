using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Domain.Common;

public abstract class Scope<TItem, TScope, TScopeHandler> : IDisposable
    where TScope : Scope<TItem, TScope, TScopeHandler>
    where TScopeHandler : IScopeHandler<TScope, TItem, TScopeHandler>, new()
{
    private bool IsDisposed { get; set; }
    protected internal List<TItem> Items { get; set; }
    private TScope? ParentScope { get; }
    private int NestedScopesCounter { get; set; }

    protected Scope()
    {
        this.Items = [];
        this.ParentScope = TScopeHandler.CurrentScope;

        if(this.ParentScope is not null)
        {
            this.ParentScope.NestedScopesCounter++;
        }

        TScopeHandler.CurrentScope = this as TScope;
    }

    public void Add(TItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        this.Items.Add(item);
    }

    public void Publish()
    {
        if(this.ParentScope is null)
        {
            foreach(TItem item in this.Items)
            {
                if(TScopeHandler.Instance.Dispatcher is null)
                {
                    throw new InvalidOperationException("Dispatcher is uninitialized.");
                }

                TScopeHandler.Instance.Dispatcher.Dispatch((dynamic)item);
            }
        }
        else
        {
            this.ParentScope.AddRange(this.Items);
        }

        this.Clear();
    }

    private void AddRange(List<TItem> items) => this.Items.AddRange(items);

    public Task PublishAsync()
    {
        List<Task> tasks = [];

        if(this.ParentScope is null)
        {
            foreach(TItem item in this.Items)
            {
                if(TScopeHandler.Instance.Dispatcher is null)
                {
                    throw new InvalidOperationException("Dispatcher is uninitialized.");
                }

                tasks.Add(TScopeHandler.Instance.Dispatcher.DispatchAsync((dynamic)item));
            }
        }
        else
        {
            this.ParentScope.AddRange(this.Items);
        }

        this.Clear();

        return Task.WhenAll(tasks);
    }

    public void Clear() => this.Items.Clear();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!this.IsDisposed)
        {
            if(this.ParentScope is not null)
            {
                this.ParentScope.NestedScopesCounter--;
            }

            if(this.NestedScopesCounter > 0)
            {
                this.Clear();
                throw new InvalidOperationException("Scope nested incorrectly.");
            }

            if(disposing)
            {
                this.Clear();
            }

            if(ReferenceEquals(this, TScopeHandler.CurrentScope))
            {
                TScopeHandler.CurrentScope = this.ParentScope;
            }
        }

        this.IsDisposed = true;
    }

    ~Scope()
    {
        this.Dispose(false);
    }
}