using System.Collections.Generic;
using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Model;

public class ValueObjectTest
{
    public static IEnumerable<TestCaseData> EqualsTestData
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(1, "AA"),
                    new FirstValueObjectFake(1, "AA"),
                    true,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(1)");
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(2, "AAB"),
                    new FirstValueObjectFake(2, "AAB"),
                    true,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(2)");
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(2, null),
                    new FirstValueObjectFake(2, null),
                    true,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(4)");
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    true,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(5)");
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3, "AABC"),
                    new ThirdValueObjectFake(2, 3, "AABC"),
                    true,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(6)");
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 3.1, null),
                    new ThirdValueObjectFake(1, 3.1, null),
                    true,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(7)");
            yield return new TestCaseData(
                new object[]
                {
                    new FourthValueObjectFake(1, 3.1, null, true),
                    new FourthValueObjectFake(1, 3.1, null, true),
                    true,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(8)");
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(1, "AA"),
                    new FirstValueObjectFake(2, "AA"),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(9)");
            yield return new TestCaseData(
                new object?[] { new FirstValueObjectFake(1, "AA"), null, false }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(10)");
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(1, "AA"),
                    new FirstValueObjectFake(1, "AB"),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(11)");
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(1, "AA"),
                    new FirstValueObjectFake(2, null),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(12)");
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(1, "AA"),
                    new ThirdValueObjectFake(1, 0, "AA"),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(13)");
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 0, "AA"),
                    new ThirdValueObjectFake(2, 0, "AA"),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(14)");
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 0, "AA"),
                    new ThirdValueObjectFake(1, 0, "AB"),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(15)");
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 0, "AA"),
                    new ThirdValueObjectFake(2, 1, null),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(16)");
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 0, "AA"),
                    new ThirdValueObjectFake(1, 0, null),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(17)");
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 0, null),
                    new ThirdValueObjectFake(1, 0, "AA"),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(18)");
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 0, "AA"),
                    new FirstValueObjectFake(1, "AA"),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(19)");
            yield return new TestCaseData(
                new object[]
                {
                    new FourthValueObjectFake(1, 0, "AA", false),
                    new FourthValueObjectFake(1, 0, "AA", true),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(20)");
            yield return new TestCaseData(
                new object[]
                {
                    new FourthValueObjectFake(1, 0, "AA", true),
                    new FourthValueObjectFake(1, 0, "AA", false),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(21)");
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(1, "AA"),
                    new SecondValueObjectFake(1, "AA"),
                    false,
                }
            ).SetName($"{nameof(TestEquals_WhenValueObjectGiven_ThenMembersAreComapred)}(22)");
        }
    }

    public static IEnumerable<TestCaseData> EqualsOperatorTestData
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    true,
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3.4, "AAC"),
                    new ThirdValueObjectFake(2, 3.4, "AAC"),
                    true,
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(2)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3.5, "AAC"),
                    new ThirdValueObjectFake(2, 3.5, "AAC"),
                    true,
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(3)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3.1, "AAB"),
                    new ThirdValueObjectFake(2, 3.1, "AAB"),
                    true,
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(4)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    new ThirdValueObjectFake(1, 3.2, "AAB"),
                    false,
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(5)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(3, 3.5, "AAC"),
                    new ThirdValueObjectFake(2, 3.4, "AAC"),
                    false,
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(6)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(3, 3.4, "AAD"),
                    new ThirdValueObjectFake(2, 3.5, "AAC"),
                    false,
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(7)"
            );
            yield return new TestCaseData(
                new object?[] { new ThirdValueObjectFake(2, 3.1, "AAB"), null, false }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(8)"
            );
            yield return new TestCaseData(
                new object?[] { null, new ThirdValueObjectFake(2, 3.1, "AAB"), false }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(9)"
            );
            yield return new TestCaseData(new object?[] { null, null, true }).SetName(
                $"{nameof(TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(10)"
            );
        }
    }

    public static IEnumerable<TestCaseData> NotEqualsOperatorTestData
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    false,
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3.4, "AAC"),
                    new ThirdValueObjectFake(2, 3.4, "AAC"),
                    false,
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(2)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3.5, "AAC"),
                    new ThirdValueObjectFake(2, 3.5, "AAC"),
                    false,
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(3)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3.1, "AAB"),
                    new ThirdValueObjectFake(2, 3.1, "AAB"),
                    false,
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(4)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    new ThirdValueObjectFake(1, 3.2, "AAB"),
                    true,
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(5)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(3, 3.5, "AAC"),
                    new ThirdValueObjectFake(2, 3.4, "AAC"),
                    true,
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(6)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(3, 3.4, "AAD"),
                    new ThirdValueObjectFake(2, 3.5, "AAC"),
                    true,
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(7)"
            );
            yield return new TestCaseData(
                new object?[] { new ThirdValueObjectFake(2, 3.1, "AAB"), null, true }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(8)"
            );
            yield return new TestCaseData(
                new object?[] { null, new ThirdValueObjectFake(2, 3.1, "AAB"), true }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(9)"
            );
            yield return new TestCaseData(new object?[] { null, null, false }).SetName(
                $"{nameof(TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred)}(10)"
            );
        }
    }

    public static IEnumerable<TestCaseData> GetHashCodeTestData
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(1, "AA"),
                    new FirstValueObjectFake(1, "AA"),
                    true,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(2, "AAB"),
                    new FirstValueObjectFake(2, "AAB"),
                    true,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(2, null),
                    new FirstValueObjectFake(2, null),
                    true,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    true,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3, "AABC"),
                    new ThirdValueObjectFake(2, 3, "AABC"),
                    true,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(1, "AA"),
                    new FirstValueObjectFake(1, "AC"),
                    false,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(3, "AAB"),
                    new FirstValueObjectFake(2, "AAB"),
                    false,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(2, null),
                    new FirstValueObjectFake(2, "AS"),
                    false,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(1, 3.2, "AAB"),
                    new ThirdValueObjectFake(1, 3.1, "AAB"),
                    false,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3, "AABC"),
                    new ThirdValueObjectFake(2, 33, "AAB"),
                    false,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new ThirdValueObjectFake(2, 3, "AABC"),
                    new FirstValueObjectFake(2, "AAB"),
                    false,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new FirstValueObjectFake(2, null),
                    new ThirdValueObjectFake(2, 3, "AABC"),
                    false,
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[] { new FirstValueObjectFake(2, null), "1", false }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[] { new ThirdValueObjectFake(2, 3, "AABC"), 2, false }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned)}(1)"
            );
        }
    }

    [TestCaseSource(nameof(EqualsTestData))]
    public void TestEquals_WhenValueObjectGiven_ThenMembersAreComapred(
        object lhsValueObject,
        object rhsValueObject,
        bool expectedEqualsResult
    ) => Assert.That(lhsValueObject.Equals(rhsValueObject), Is.EqualTo(expectedEqualsResult));

    [TestCaseSource(nameof(EqualsOperatorTestData))]
    public void TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred(
        ThirdValueObjectFake lhsValueObject,
        ThirdValueObjectFake rhsValueObject,
        bool expectedEqualsResult
    ) => Assert.That(lhsValueObject != rhsValueObject, Is.Not.EqualTo(expectedEqualsResult));

    [TestCaseSource(nameof(NotEqualsOperatorTestData))]
    public void TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred(
        ThirdValueObjectFake lhsValueObject,
        ThirdValueObjectFake rhsValueObject,
        bool expectedEqualsResult
    ) => Assert.That(lhsValueObject != rhsValueObject, Is.EqualTo(expectedEqualsResult));

    [TestCaseSource(nameof(GetHashCodeTestData))]
    public void TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned(
        object lhsValueObject,
        object rhsObject,
        bool expectedEqualsHashCodeResult
    )
    {
        Assert.That(
            lhsValueObject.GetHashCode() == rhsObject.GetHashCode(),
            Is.EqualTo(expectedEqualsHashCodeResult)
        );
    }
}
