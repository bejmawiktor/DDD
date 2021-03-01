using DDD.Domain.Model;
using System;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class ValidatorFake : IValidator<int?>
    {
        public void Validate(int? validatedObject)
        {
            if(validatedObject == null)
            {
                throw new ArgumentNullException("field1");
            }
        }
    }
}