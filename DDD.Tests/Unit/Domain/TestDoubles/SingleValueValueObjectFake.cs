using DDD.Domain.Model;
using System;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class SingleValueValueObjectFake : ValueObject<string>
    {
        public SingleValueValueObjectFake(string value) : base(value)
        {
        }

        protected override void ValidateValue(string value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }
    }
}