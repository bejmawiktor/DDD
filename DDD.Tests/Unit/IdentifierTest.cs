using DDD.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DDD.Tests.Unit
{
    [TestFixture]
    public class IdentifierTest
    {
        public class TestId : Identifier<string>
        {
            public TestId(string value) : base(value)
            {
            }

            protected override void ValidateValue(string value)
            {
                if(value == string.Empty)
                {
                    throw new ArgumentException("Id could not be empty.");
                }
            }
        }

        public static IEnumerable<object[]> EqualityByMethodTestData()
        {
            yield return new object[]
            {
                new TestId("1"),
                new TestId("1"),
                true
            };
        }

        [Test]
        public void TestValueValidation()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("value"),
                () => new TestId(null));
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Id could not be empty."),
                () => new TestId(""));
            Assert.DoesNotThrow(() => new TestId("1"));
        }

        [Test]
        public void TestValueSetting()
        {
            TestId testId = new TestId("1");

            Assert.That(testId.Value, Is.EqualTo("1"));
        }
    }
}