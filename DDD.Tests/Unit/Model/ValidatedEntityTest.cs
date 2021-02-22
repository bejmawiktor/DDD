using DDD.Tests.Unit.TestDoubles;
using NUnit.Framework;
using System;

namespace DDD.Tests.Unit.Model
{
    [TestFixture]
    public class ValidatedEntityTest
    {
        [Test]
        public void TestConstructing_WhenValidatedObjectIsNotValid_ThenValidationExceptionsAreThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("field1"),
                () => new ValidatedEntityFake("1", null));
        }
    }
}