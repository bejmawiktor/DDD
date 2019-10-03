using DDD.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DDD.Tests.Unit
{
    public class EntityTest
    {
        private class StringEntity : Entity<string>
        {
            public StringEntity(string id) : base(id)
            {
            }
        }

        private class IntEntity : Entity<int>
        {
            public IntEntity(int id) : base(id)
            {
            }
        }

        public static IEnumerable<object[]> EqualityTestData
        {
            get
            {
                yield return new object[]
                {
                    new IntEntity(1),
                    new IntEntity(1)
                };
                yield return new object[]
                {
                    new IntEntity(123),
                    new IntEntity(123)
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("1")
                };
                yield return new object[]
                {
                    new StringEntity("123"),
                    new StringEntity("123")
                };
            }
        }

        public static IEnumerable<object[]> InequalityTestData
        {
            get
            {
                yield return new object[]
                {
                    new IntEntity(1),
                    new IntEntity(2)
                };
                yield return new object[]
                {
                    new IntEntity(1),
                    new StringEntity("1")
                };
                yield return new object[]
                {
                    new IntEntity(1),
                    null
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("12"),
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    null,
                };
            }
        }

        [Test]
        public void TestEntityReferenceTypeValidation()
        {
            Assert.Throws<ArgumentNullException>(() => new StringEntity(null));
        }

        [TestCaseSource(nameof(EqualityTestData))]
        public void TestEqualityUsingEqualsMethod(object lhsEntity, object rhsEntity)
        {
            Assert.That(lhsEntity.Equals(rhsEntity), Is.True);
        }

        [TestCaseSource(nameof(InequalityTestData))]
        public void TestInequalityUsingEqualsMethod(object lhsEntity, object rhsEntity)
        {
            Assert.That(lhsEntity.Equals(rhsEntity), Is.False);
        }

        [Test]
        public void TestEqualityUsingEqualsOperator()
        {
            IntEntity lhsIntEntity = new IntEntity(1);
            IntEntity rhsIntEntity = new IntEntity(1);
            StringEntity lhsStringEntity = new StringEntity("1");
            StringEntity rhsStringEntity = new StringEntity("1");

            Assert.That(lhsIntEntity == rhsIntEntity, Is.True);
            Assert.That((StringEntity)null == (StringEntity)null, Is.True);
            Assert.That(lhsStringEntity == rhsStringEntity, Is.True);
            Assert.That((IntEntity)null == (IntEntity)null, Is.True);
        }

        [Test]
        public void TestInequalityUsingEqualsOperator()
        {
            IntEntity lhsIntEntity = new IntEntity(1);
            IntEntity rhsIntEntity = new IntEntity(2);
            StringEntity lhsStringEntity = new StringEntity("1");
            StringEntity rhsStringEntity = new StringEntity("2");

            Assert.That(lhsIntEntity == rhsIntEntity, Is.False);
            Assert.That(null == rhsIntEntity, Is.False);
            Assert.That(lhsIntEntity == null, Is.False);
            Assert.That(lhsStringEntity == rhsStringEntity, Is.False);
            Assert.That(lhsStringEntity == null, Is.False);
            Assert.That(null == rhsStringEntity, Is.False);
        }

        [Test]
        public void TestEqualityUsingNotEqualsOperator()
        {
            IntEntity lhsIntEntity = new IntEntity(1);
            IntEntity rhsIntEntity = new IntEntity(1);
            StringEntity lhsStringEntity = new StringEntity("1");
            StringEntity rhsStringEntity = new StringEntity("1");

            Assert.That(lhsIntEntity != rhsIntEntity, Is.False);
            Assert.That((StringEntity)null != (StringEntity)null, Is.False);
            Assert.That(lhsStringEntity != rhsStringEntity, Is.False);
            Assert.That((IntEntity)null != (IntEntity)null, Is.False);
        }

        [Test]
        public void TestInequalityUsingNotEqualsOperator()
        {
            IntEntity lhsIntEntity = new IntEntity(1);
            IntEntity rhsIntEntity = new IntEntity(2);
            StringEntity lhsStringEntity = new StringEntity("1");
            StringEntity rhsStringEntity = new StringEntity("2");

            Assert.That(lhsIntEntity != rhsIntEntity, Is.True);
            Assert.That(null != rhsIntEntity, Is.True);
            Assert.That(lhsIntEntity != null, Is.True);
            Assert.That(lhsStringEntity != rhsStringEntity, Is.True);
            Assert.That(lhsStringEntity != null, Is.True);
            Assert.That(null != rhsStringEntity, Is.True);
        }

        [Test]
        public void TestHashCodeGetting()
        {
            int intId = 20;
            string stringId = "AA";
            IntEntity intEntity = new IntEntity(intId);
            StringEntity stringEntity = new StringEntity(stringId);

            int intHashCode = intEntity.GetHashCode();
            int stringHashCode = stringEntity.GetHashCode();

            Assert.That(intHashCode, Is.EqualTo(intId.GetHashCode() + intEntity.GetType().GetHashCode() + 2108858624));
            Assert.That(stringHashCode, Is.EqualTo(stringId.GetHashCode() + stringEntity.GetType().GetHashCode() + 2108858624));
        }
    }
}