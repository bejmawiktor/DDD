using System;
using System.Collections.Generic;
using DDD.Domain.Common;

namespace DDD.Tests.Unit.Domain.TestDoubles;

internal class ScopeFake : Scope<string, ScopeFake, ScopeHandlerFake>
{
    public new IReadOnlyCollection<string> Items => base.Items.AsReadOnly();
}
