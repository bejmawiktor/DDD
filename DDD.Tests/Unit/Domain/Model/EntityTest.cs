using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace  DDD.Tests.Unit.Domain.Model
{
    public class EntityTest
    {
        public static IEnumerable<TestCaseData> ObjectEqualsTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    new IntIdEntityStub(1),
                    new IntIdEntityStub(1),
                    true
                }).SetName($"{nameof(TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared)}(1)");
                yield return new TestCaseData(new object[]
                {
                    new IntIdEntityStub(123),
                    new IntIdEntityStub(123),
                    true
                }).SetName($"{nameof(TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared)}(2)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("1"),
                    true
                }).SetName($"{nameof(TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared)}(3)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("123"),
                    new StringEntityStub("123"),
                    true
                }).SetName($"{nameof(TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared)}(4)");
                yield return new TestCaseData(new object[]
                {
                    new IntIdEntityStub(1),
                    new IntIdEntityStub(2),
                    false
                }).SetName($"{nameof(TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared)}(5)");
                yield return new TestCaseData(new object[]
                {
                    new IntIdEntityStub(1),
                    new StringEntityStub("1"),
                    false
                }).SetName($"{nameof(TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared)}(6)");
                yield return new TestCaseData(new object[]
                {
                    new IntIdEntityStub(1),
                    null,
                    false
                }).SetName($"{nameof(TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared)}(7)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("12"),
                    false,
                }).SetName($"{nameof(TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared)}(8)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    null,
                    false,
                }).SetName($"{nameof(TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared)}(9)");
            }
        }

        public static IEnumerable<TestCaseData> EntityEqualsTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("1"),
                    true
                }).SetName($"{nameof(TestEqualsWithEntity_WhenEntityGiven_ThenIdsAreCompared)}(1)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("123"),
                    new StringEntityStub("123"),
                    true
                }).SetName($"{nameof(TestEqualsWithEntity_WhenEntityGiven_ThenIdsAreCompared)}(2)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("12"),
                    false,
                }).SetName($"{nameof(TestEqualsWithEntity_WhenEntityGiven_ThenIdsAreCompared)}(3)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    null,
                    false,
                }).SetName($"{nameof(TestEqualsWithEntity_WhenEntityGiven_ThenIdsAreCompared)}(4)");
            }
        }

        public static IEnumerable<TestCaseData> EqualsOperatorTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("1"),
                    true
                }).SetName($"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(1)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("123"),
                    new StringEntityStub("123"),
                    true
                }).SetName($"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(2)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("12"),
                    false,
                }).SetName($"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(3)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    null,
                    false,
                }).SetName($"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(4)");
                yield return new TestCaseData(new object[]
                {
                    null,
                    new StringEntityStub("1"),
                    false,
                }).SetName($"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(5)");
                yield return new TestCaseData(new object[]
                {
                    null,
                    null,
                    true,
                }).SetName($"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(6)");
            }
        }

        public static IEnumerable<TestCaseData> NotEqualsOperatorTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("1"),
                    false
                }).SetName($"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(1)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("123"),
                    new StringEntityStub("123"),
                    false
                }).SetName($"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(2)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("12"),
                    true,
                }).SetName($"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(3)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    null,
                    true,
                }).SetName($"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(4)");
                yield return new TestCaseData(new object[]
                {
                    null,
                    new StringEntityStub("1"),
                    true,
                }).SetName($"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(5)");
                yield return new TestCaseData(new object[]
                {
                    null,
                    null,
                    false,
                }).SetName($"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(6)");
            }
        }

        public static IEnumerable<TestCaseData> GetHashCodeTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    new IntIdEntityStub(1),
                    new IntIdEntityStub(1),
                    true
                }).SetName($"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    new IntIdEntityStub(123),
                    new IntIdEntityStub(123),
                    true
                }).SetName($"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("1"),
                    true
                }).SetName($"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("123"),
                    new StringEntityStub("123"),
                    true
                }).SetName($"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    new IntIdEntityStub(1),
                    new IntIdEntityStub(2),
                    false
                }).SetName($"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(5)");
                yield return new TestCaseData(new object[]
                {
                    new IntIdEntityStub(1),
                    new StringEntityStub("1"),
                    false
                }).SetName($"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(6)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    new StringEntityStub("12"),
                    false,
                }).SetName($"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(7)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    "1",
                    false,
                }).SetName($"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(8)");
                yield return new TestCaseData(new object[]
                {
                    new StringEntityStub("1"),
                    2,
                    false,
                }).SetName($"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(9)");
            }
        }

        [Test]
        public void TestConstructing_WhenNullIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("id"),
                () => new StringEntityStub(null));
        }

        [Test]
        public void TestConstructing_WhenIdGiven_ThenIdIsSet()
        {
            StringEntityStub stringEntityStub = new StringEntityStub("1");

            Assert.That(stringEntityStub.Id, Is.EqualTo("1"));
        }

        [TestCaseSource(nameof(ObjectEqualsTestData))]
        public void TestEqualsWithObject_WhenEntityGiven_ThenIdsAreCompared(
            object lhsEntity,
            object rhsEntity,
            bool expectedEqualsResult)
        {
            Assert.That(lhsEntity.Equals(rhsEntity), Is.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(EntityEqualsTestData))]
        public void TestEqualsWithEntity_WhenEntityGiven_ThenIdsAreCompared(
            StringEntityStub lhsEntity,
            StringEntityStub rhsEntity,
            bool expectedEqualsResult)
        {
            Assert.That(lhsEntity.Equals(rhsEntity), Is.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(EqualsOperatorTestData))]
        public void TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared(
            StringEntityStub lhsEntity,
            StringEntityStub rhsEntity,
            bool expectedEqualsResult)
        {
            Assert.That(lhsEntity == rhsEntity, Is.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(NotEqualsOperatorTestData))]
        public void TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared(
            StringEntityStub lhsEntity,
            StringEntityStub rhsEntity,
            bool expectedEqualsResult)
        {
            Assert.That(lhsEntity != rhsEntity, Is.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(GetHashCodeTestData))]
        public void TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned(
            object lhsEntity,
            object rhsEntity,
            bool expectedEqualsHashCodeResult)
        {
            Assert.That(lhsEntity.GetHashCode() == rhsEntity.GetHashCode(), Is.EqualTo(expectedEqualsHashCodeResult));
        }
    }
}