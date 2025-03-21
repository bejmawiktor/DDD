﻿using System.Collections.Generic;
using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class ValidatedValueObjectFake : ValueObject<int?, ValidatorFake>
{
    public int? Field1 { get; }

    public ValidatedValueObjectFake(int? field1)
        : base(field1)
    {
        this.Field1 = field1;
    }

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Field1;
    }
}
