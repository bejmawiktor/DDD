﻿using System.Threading;
using Utils.Disposable;

namespace DDD.Tests.Unit.Domain.TestDoubles;

internal class DisposableScopeContainerFake
    : IScopeContainer<DisposableScopeFake, DisposableScopeContainerFake>
{
    private static readonly AsyncLocal<DisposableScopeFake?> localEventsScope = new();

    public static DisposableScopeFake? CurrentScope
    {
        get => DisposableScopeContainerFake.localEventsScope.Value;
        set => DisposableScopeContainerFake.localEventsScope.Value = value;
    }
}
