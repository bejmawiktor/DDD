using System;
using System.Collections.Generic;
using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Model;

public class EntityTest
{
    public static IEnumerable<TestCaseData> ObjectEqualsTestData
    {
        get
        {
            yield return new TestCaseData(
                new IntIdEntityStub(1),
                new IntIdEntityStub(1),
                true
            ).SetName($"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(1)");
            yield return new TestCaseData(
                new IntIdEntityStub(123),
                new IntIdEntityStub(123),
                true
            ).SetName($"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(2)");
            yield return new TestCaseData(
                new StringEntityStub("1"),
                new StringEntityStub("1"),
                true
            ).SetName($"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(3)");
            yield return new TestCaseData(
                new StringEntityStub("123"),
                new StringEntityStub("123"),
                true
            ).SetName($"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(4)");
            yield return new TestCaseData(
                new IntIdEntityStub(1),
                new IntIdEntityStub(2),
                false
            ).SetName($"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(5)");
            yield return new TestCaseData(
                new IntIdEntityStub(1),
                new StringEntityStub("1"),
                false
            ).SetName($"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(6)");
            yield return new TestCaseData(new IntIdEntityStub(1), null, false).SetName(
                $"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(7)"
            );
            yield return new TestCaseData(
                new StringEntityStub("1"),
                new StringEntityStub("12"),
                false
            ).SetName($"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(8)");
            yield return new TestCaseData(new StringEntityStub("1"), null, false).SetName(
                $"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(9)"
            );
            yield return new TestCaseData(
                new StringEntityStub("1"),
                new OtherStringEntityStub("1"),
                false
            ).SetName($"{nameof(TestEquals_WhenEntityGiven_ThenIdsAreCompared)}(10)");
        }
    }

    public static IEnumerable<TestCaseData> EqualsOperatorTestData
    {
        get
        {
            yield return new TestCaseData(
                new StringEntityStub("1"),
                new StringEntityStub("1"),
                true
            ).SetName($"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(1)");
            yield return new TestCaseData(
                new StringEntityStub("123"),
                new StringEntityStub("123"),
                true
            ).SetName($"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(2)");
            yield return new TestCaseData(
                new StringEntityStub("1"),
                new StringEntityStub("12"),
                false
            ).SetName($"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(3)");
            yield return new TestCaseData(new StringEntityStub("1"), null, false).SetName(
                $"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(4)"
            );
            yield return new TestCaseData(null, new StringEntityStub("1"), false).SetName(
                $"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(5)"
            );
            yield return new TestCaseData(null, null, true).SetName(
                $"{nameof(TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(6)"
            );
        }
    }

    public static IEnumerable<TestCaseData> NotEqualsOperatorTestData
    {
        get
        {
            yield return new TestCaseData(
                new StringEntityStub("1"),
                new StringEntityStub("1"),
                false
            ).SetName($"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(1)");
            yield return new TestCaseData(
                new StringEntityStub("123"),
                new StringEntityStub("123"),
                false
            ).SetName($"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(2)");
            yield return new TestCaseData(
                new StringEntityStub("1"),
                new StringEntityStub("12"),
                true
            ).SetName($"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(3)");
            yield return new TestCaseData(new StringEntityStub("1"), null, true).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(4)"
            );
            yield return new TestCaseData(null, new StringEntityStub("1"), true).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(5)"
            );
            yield return new TestCaseData(null, null, false).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared)}(6)"
            );
        }
    }

    public static IEnumerable<TestCaseData> GetHashCodeTestData
    {
        get
        {
            yield return new TestCaseData(
                new IntIdEntityStub(1),
                new IntIdEntityStub(1),
                true
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new IntIdEntityStub(123),
                new IntIdEntityStub(123),
                true
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(2)"
            );
            yield return new TestCaseData(
                new StringEntityStub("1"),
                new StringEntityStub("1"),
                true
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(3)"
            );
            yield return new TestCaseData(
                new StringEntityStub("123"),
                new StringEntityStub("123"),
                true
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(4)"
            );
            yield return new TestCaseData(
                new IntIdEntityStub(1),
                new IntIdEntityStub(2),
                false
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(5)"
            );
            yield return new TestCaseData(
                new IntIdEntityStub(1),
                new StringEntityStub("1"),
                false
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(6)"
            );
            yield return new TestCaseData(
                new StringEntityStub("1"),
                new StringEntityStub("12"),
                false
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(7)"
            );
            yield return new TestCaseData(new StringEntityStub("1"), "1", false).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(8)"
            );
            yield return new TestCaseData(new StringEntityStub("1"), 2, false).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned)}(9)"
            );
        }
    }

    [Test]
    public void TestConstructing_WhenNullIdGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("id"),
            () => new StringEntityStub(null!)
        );
    }

    [Test]
    public void TestSet_WhenNullIdGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("id"),
            () => new StringEntityStub("AAA").Id = null!
        );
    }

    [Test]
    public void TestSet_WhenProperIdGiven_ThenIdIsSet()
    {
        StringEntityStub entity = new("AAA") { Id = "BBB" };

        Assert.That(entity.Id, Is.EqualTo("BBB"));
    }

    [Test]
    public void TestConstructing_WhenIdGiven_ThenIdIsSet()
    {
        StringEntityStub stringEntityStub = new("1");

        Assert.That(stringEntityStub.Id, Is.EqualTo("1"));
    }

    [TestCaseSource(nameof(ObjectEqualsTestData))]
    public void TestEquals_WhenEntityGiven_ThenIdsAreCompared(
        object lhsEntity,
        object rhsEntity,
        bool expectedEqualsResult
    ) => Assert.That(lhsEntity.Equals(rhsEntity), Is.EqualTo(expectedEqualsResult));

    [TestCaseSource(nameof(EqualsOperatorTestData))]
    public void TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared(
        StringEntityStub lhsEntity,
        StringEntityStub rhsEntity,
        bool expectedEqualsResult
    ) => Assert.That(lhsEntity == rhsEntity, Is.EqualTo(expectedEqualsResult));

    [TestCaseSource(nameof(NotEqualsOperatorTestData))]
    public void TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared(
        StringEntityStub lhsEntity,
        StringEntityStub rhsEntity,
        bool expectedEqualsResult
    ) => Assert.That(lhsEntity != rhsEntity, Is.EqualTo(expectedEqualsResult));

    [TestCaseSource(nameof(GetHashCodeTestData))]
    public void TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned(
        object lhsEntity,
        object rhsEntity,
        bool expectedEqualsHashCodeResult
    )
    {
        Assert.That(
            lhsEntity.GetHashCode() == rhsEntity.GetHashCode(),
            Is.EqualTo(expectedEqualsHashCodeResult)
        );
    }
}
