using DDD.Model;
using NUnit.Framework;
using System;

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
        }

        [Test]
        public void TestIdentifierReferenceTypeValidation()
        {
            TestId testId = new TestId("1");

            Assert.Throws<ArgumentNullException>(() => new TestId(null));
            Assert.That(testId.Value, Is.EqualTo("1"));
        }
    }
}