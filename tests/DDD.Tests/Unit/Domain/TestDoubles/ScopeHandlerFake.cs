using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Common;
using static System.Formats.Asn1.AsnWriter;

namespace DDD.Tests.Unit.Domain.TestDoubles;

internal class ScopeHandlerFake : IScopeHandler<ScopeFake, string, ScopeHandlerFake>
{
    private static readonly Lazy<ScopeHandlerFake> instance = new(() => new ScopeHandlerFake());

    private static readonly AsyncLocal<ScopeFake?> localScope = new();

    static ScopeFake? IScopeHandler<ScopeFake, string, ScopeHandlerFake>.CurrentScope
    {
        get => ScopeHandlerFake.CurrentScope;
        set => ScopeHandlerFake.CurrentScope = value;
    }

    static ScopeHandlerFake IScopeHandler<ScopeFake, string, ScopeHandlerFake>.Instance =>
        ScopeHandlerFake.Instance;

    public static ScopeFake? CurrentScope
    {
        get => localScope.Value;
        internal set => localScope.Value = value;
    }

    public static ScopeHandlerFake Instance => instance.Value;

    public IDispatcher<string>? Dispatcher { get; set; }

    public ScopeHandlerFake() { }
}
