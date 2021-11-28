using DDD.Domain.Model;
using System;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class OneValueValueObjectFake : ValueObject<string, OneValueValueObjectFake>
    {
        public OneValueValueObjectFake(string value) : base(value)
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