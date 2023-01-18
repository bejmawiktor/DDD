using DDD.Domain.Model;
using System;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class ValidatorFake : IValidator<int?>
    {
        public void Validate(int? validatedObject)
        {
            ArgumentNullException.ThrowIfNull(validatedObject, "field1");
        }
    }
}