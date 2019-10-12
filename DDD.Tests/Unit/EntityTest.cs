using DDD.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DDD.Tests.Unit
{
    public class EntityTest
    {
        public class StringEntity : Entity<string>
        {
            public StringEntity(string id) : base(id)
            {
            }
        }

        public class IntEntity : Entity<int>
        {
            public IntEntity(int id) : base(id)
            {
            }
        }

        public static IEnumerable<object[]> EqualityByEqualsMethodTestData
        {
            get
            {
                yield return new object[]
                {
                    new IntEntity(1),
                    new IntEntity(1),
                    true
                };
                yield return new object[]
                {
                    new IntEntity(123),
                    new IntEntity(123),
                    true
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("1"),
                    true
                };
                yield return new object[]
                {
                    new StringEntity("123"),
                    new StringEntity("123"),
                    true
                };
                yield return new object[]
                {
                    new IntEntity(1),
                    new IntEntity(2),
                    false
                };
                yield return new object[]
                {
                    new IntEntity(1),
                    new StringEntity("1"),
                    false
                };
                yield return new object[]
                {
                    new IntEntity(1),
                    null,
                    false
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("12"),
                    false,
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    null,
                    false,
                };
            }
        }

        public static IEnumerable<object[]> EqualityByEqualsEntityMethodTestData
        {
            get
            {
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("1"),
                    true
                };
                yield return new object[]
                {
                    new StringEntity("123"),
                    new StringEntity("123"),
                    true
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("12"),
                    false,
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    null,
                    false,
                };
            }
        }

        public static IEnumerable<object[]> EqualityByOperatorsTestData
        {
            get
            {
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("1"),
                    true
                };
                yield return new object[]
                {
                    new StringEntity("123"),
                    new StringEntity("123"),
                    true
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("12"),
                    false,
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    null,
                    false,
                };
                yield return new object[]
                {
                    null,
                    new StringEntity("1"),
                    false,
                };
                yield return new object[]
                {
                    null,
                    null,
                    true,
                };
            }
        }

        public static IEnumerable<object[]> GetHashCodeTestData
        {
            get
            {
                yield return new object[]
                {
                    new IntEntity(1),
                    new IntEntity(1),
                    true
                };
                yield return new object[]
                {
                    new IntEntity(123),
                    new IntEntity(123),
                    true
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("1"),
                    true
                };
                yield return new object[]
                {
                    new StringEntity("123"),
                    new StringEntity("123"),
                    true
                };
                yield return new object[]
                {
                    new IntEntity(1),
                    new IntEntity(2),
                    false
                };
                yield return new object[]
                {
                    new IntEntity(1),
                    new StringEntity("1"),
                    false
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    new StringEntity("12"),
                    false,
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    "1",
                    false,
                };
                yield return new object[]
                {
                    new StringEntity("1"),
                    2,
                    false,
                };
            }
        }

        [Test]
        public void TestValidation()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("id"),
                () => new StringEntity(null));
        }

        [Test]
        public void TestCreatingEntity()
        {
            StringEntity stringEntity = new StringEntity("1");

            Assert.That(stringEntity.Id, Is.EqualTo("1"));
        }

        [TestCaseSource(nameof(EqualityByEqualsMethodTestData))]
        public void TestEqualityUsingEqualsObjectMethod(object lhsEntity, object rhsEntity, bool expectedEqualsResult)
        {
            Assert.That(lhsEntity.Equals(rhsEntity), Is.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(EqualityByEqualsEntityMethodTestData))]
        public void TestEqualityUsingEqualsEntityMethod(
            StringEntity lhsEntity,
            StringEntity rhsEntity,
            bool expectedEqualsResult)
        {
            Assert.That(lhsEntity.Equals(rhsEntity), Is.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(EqualityByOperatorsTestData))]
        public void TestEqualityUsingEqualsOperators(
            StringEntity lhsEntity,
            StringEntity rhsEntity,
            bool expectedEqualsResult)
        {
            Assert.That(lhsEntity == rhsEntity, Is.EqualTo(expectedEqualsResult));
            Assert.That(lhsEntity != rhsEntity, Is.Not.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(GetHashCodeTestData))]
        public void TestGettingHashCode(object lhsEntity, object rhsEntity, bool expectedEqualsHashCodeResult)
        {
            Assert.That(lhsEntity.GetHashCode() == rhsEntity.GetHashCode(), Is.EqualTo(expectedEqualsHashCodeResult));
        }
    }
}