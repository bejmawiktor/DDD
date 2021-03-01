using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;
using System;

namespace  DDD.Tests.Unit.Domain.Model
{
    public class ValidatedValueObjectTest
    {
        [Test]
        public void TestConstructing_WhenValidatedObjectIsNotValid_ThenValidationExceptionsAreThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("field1"),
                () => new ValidatedValueObjectFake(null));
        }
    }
}