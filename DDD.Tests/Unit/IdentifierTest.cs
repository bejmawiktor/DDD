using DDD.Model;
using NUnit.Framework;
using System;

namespace DDD.Tests.Unit
{
    [TestFixture]
    public class IdentifierTest
    {
        public class TestId : Identifier<string, TestId>
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

        public class TestId2 : Identifier<int, TestId2>
        {
            public TestId2(int value) : base(value)
            {
            }

            protected override void ValidateValue(int value)
            {
                if(value <= 0)
                {
                    throw new ArgumentException("Id must be greater than 0.");
                }
            }
        }

        [Test]
        public void TestIdentifierReferenceTypeValidation()
        {
            TestId testId = new TestId("1");

            Assert.Throws<ArgumentNullException>(() => new TestId(null));
            Assert.Throws<ArgumentException>(() => new TestId(""));
            Assert.That(testId.Value, Is.EqualTo("1"));
        }

        [Test]
        public void TestEquality()
        {
            Assert.That(new TestId("1"), Is.EqualTo(new TestId("1")));
            Assert.That(new TestId2(1), Is.EqualTo(new TestId2(1)));
            Assert.That(new TestId2(1), Is.Not.EqualTo(new TestId("1")));
            Assert.That(new TestId2(1), Is.Not.EqualTo(new TestId2(2)));
            Assert.That(new TestId("1"), Is.Not.EqualTo(new TestId("2")));
        }
    }
}