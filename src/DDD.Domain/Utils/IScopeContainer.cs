namespace DDD.Domain.Utils;

public interface IScopeContainer<TScope, TScopeHandler>
    where TScope : DisposableScope<TScope, TScopeHandler>
    where TScopeHandler : IScopeContainer<TScope, TScopeHandler>
{
    protected internal static abstract TScope? CurrentScope { get; set; }
}
