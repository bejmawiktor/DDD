using System;
using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class ValidatorFake : IValidator<int?>
{
    public void Validate(int? validatedObject) =>
        ArgumentNullException.ThrowIfNull(validatedObject, "field1");
}
