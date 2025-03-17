using System;

namespace DDD.Domain.Utils;

public abstract class DisposableScope<TDisposableScope, TScopeContainer> : IDisposable
    where TDisposableScope : DisposableScope<TDisposableScope, TScopeContainer>
    where TScopeContainer : IScopeContainer<TDisposableScope, TScopeContainer>
{
    private bool IsDisposed { get; set; }
    protected internal TDisposableScope? ParentScope { get; }
    private int NestedScopesCounter { get; set; }

    protected DisposableScope()
    {
        this.ParentScope = TScopeContainer.CurrentScope;

        if (this.ParentScope is not null)
        {
            this.ParentScope.NestedScopesCounter++;
        }

        TScopeContainer.CurrentScope = this as TDisposableScope;
    }

    public virtual void Clear() { }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.IsDisposed)
        {
            if (this.ParentScope is not null)
            {
                this.ParentScope.NestedScopesCounter--;
            }

            if (this.NestedScopesCounter > 0)
            {
                this.Clear();
                throw new InvalidOperationException("Scope nested incorrectly.");
            }

            if (disposing)
            {
                this.Clear();
            }

            if (ReferenceEquals(this, TScopeContainer.CurrentScope))
            {
                TScopeContainer.CurrentScope = this.ParentScope;
            }
        }

        this.IsDisposed = true;
    }

    ~DisposableScope()
    {
        this.Dispose(false);
    }
}
