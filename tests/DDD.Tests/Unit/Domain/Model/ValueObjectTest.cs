using DDD.Tests.Unit.Domain.TestDoubles;
using DDD.Tests.Unit.Utils;
using ValueObjectEqualsCase = (
    object LhsValueObject,
    object? RhsValueObject,
    bool ExpectedEqualsResult
);
using ValueObjectHashCodeCase = (object LhsValueObject, object RhsObject, bool ExpectedResult);
using ValueObjectOperatorCase = (
    DDD.Tests.Unit.Domain.TestDoubles.ThirdValueObjectFake? LhsValueObject,
    DDD.Tests.Unit.Domain.TestDoubles.ThirdValueObjectFake? RhsValueObject,
    bool ExpectedResult
);

namespace DDD.Tests.Unit.Domain.Model;

public class ValueObjectTest
{
    public static IEnumerable<Func<TestDataRow<ValueObjectEqualsCase>>> EqualsTestData()
    {
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new FirstValueObjectFake(1, "AA"), new FirstValueObjectFake(1, "AA"), true),
            "Equal two-member value objects"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new FirstValueObjectFake(2, "AAB"), new FirstValueObjectFake(2, "AAB"), true),
            "Equal two-member value objects (2, AAB)"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new FirstValueObjectFake(2, null), new FirstValueObjectFake(2, null), true),
            "Equal two-member value objects with null member"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                true
            ),
            "Equal three-member value objects"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new ThirdValueObjectFake(2, 3, "AABC"), new ThirdValueObjectFake(2, 3, "AABC"), true),
            "Equal three-member value objects (2, 3, AABC)"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new ThirdValueObjectFake(1, 3.1, null), new ThirdValueObjectFake(1, 3.1, null), true),
            "Equal three-member value objects with null member"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (
                new FourthValueObjectFake(1, 3.1, null, true),
                new FourthValueObjectFake(1, 3.1, null, true),
                true
            ),
            "Equal four-member value objects"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new FirstValueObjectFake(1, "AA"), new FirstValueObjectFake(2, "AA"), false),
            "Different first member"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new FirstValueObjectFake(1, "AA"), null, false),
            "Compared with null"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new FirstValueObjectFake(1, "AA"), new FirstValueObjectFake(1, "AB"), false),
            "Different second member"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new FirstValueObjectFake(1, "AA"), new FirstValueObjectFake(2, null), false),
            "Different members with null"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new FirstValueObjectFake(1, "AA"), new ThirdValueObjectFake(1, 0, "AA"), false),
            "Different value object types (First vs Third)"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new ThirdValueObjectFake(1, 0, "AA"), new ThirdValueObjectFake(2, 0, "AA"), false),
            "Different first member (three-member)"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new ThirdValueObjectFake(1, 0, "AA"), new ThirdValueObjectFake(1, 0, "AB"), false),
            "Different third member (three-member)"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new ThirdValueObjectFake(1, 0, "AA"), new ThirdValueObjectFake(2, 1, null), false),
            "Different members with null (three-member)"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new ThirdValueObjectFake(1, 0, "AA"), new ThirdValueObjectFake(1, 0, null), false),
            "Third member null on one side"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new ThirdValueObjectFake(1, 0, null), new ThirdValueObjectFake(1, 0, "AA"), false),
            "Third member null on other side"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new ThirdValueObjectFake(1, 0, "AA"), new FirstValueObjectFake(1, "AA"), false),
            "Different value object types (Third vs First)"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (
                new FourthValueObjectFake(1, 0, "AA", false),
                new FourthValueObjectFake(1, 0, "AA", true),
                false
            ),
            "Different boolean member"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (
                new FourthValueObjectFake(1, 0, "AA", true),
                new FourthValueObjectFake(1, 0, "AA", false),
                false
            ),
            "Different boolean member (reversed)"
        );
        yield return TestCase.Of<ValueObjectEqualsCase>(
            (new FirstValueObjectFake(1, "AA"), new SecondValueObjectFake(1, "AA"), false),
            "Different value object types with same members"
        );
    }

    public static IEnumerable<Func<TestDataRow<ValueObjectOperatorCase>>> EqualsOperatorTestData()
    {
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                true
            ),
            "Equal three-member value objects"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(2, 3.4, "AAC"),
                new ThirdValueObjectFake(2, 3.4, "AAC"),
                true
            ),
            "Equal three-member value objects (2, 3.4, AAC)"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(2, 3.5, "AAC"),
                new ThirdValueObjectFake(2, 3.5, "AAC"),
                true
            ),
            "Equal three-member value objects (2, 3.5, AAC)"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(2, 3.1, "AAB"),
                new ThirdValueObjectFake(2, 3.1, "AAB"),
                true
            ),
            "Equal three-member value objects (2, 3.1, AAB)"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                new ThirdValueObjectFake(1, 3.2, "AAB"),
                false
            ),
            "Different second member"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(3, 3.5, "AAC"),
                new ThirdValueObjectFake(2, 3.4, "AAC"),
                false
            ),
            "Different first and second members"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(3, 3.4, "AAD"),
                new ThirdValueObjectFake(2, 3.5, "AAC"),
                false
            ),
            "All members different"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (new ThirdValueObjectFake(2, 3.1, "AAB"), null, false),
            "Right side is null"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (null, new ThirdValueObjectFake(2, 3.1, "AAB"), false),
            "Left side is null"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (null, null, true),
            "Both sides are null"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<ValueObjectOperatorCase>>
    > NotEqualsOperatorTestData()
    {
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                false
            ),
            "Equal three-member value objects"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(2, 3.4, "AAC"),
                new ThirdValueObjectFake(2, 3.4, "AAC"),
                false
            ),
            "Equal three-member value objects (2, 3.4, AAC)"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(2, 3.5, "AAC"),
                new ThirdValueObjectFake(2, 3.5, "AAC"),
                false
            ),
            "Equal three-member value objects (2, 3.5, AAC)"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(2, 3.1, "AAB"),
                new ThirdValueObjectFake(2, 3.1, "AAB"),
                false
            ),
            "Equal three-member value objects (2, 3.1, AAB)"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                new ThirdValueObjectFake(1, 3.2, "AAB"),
                true
            ),
            "Different second member"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(3, 3.5, "AAC"),
                new ThirdValueObjectFake(2, 3.4, "AAC"),
                true
            ),
            "Different first and second members"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (
                new ThirdValueObjectFake(3, 3.4, "AAD"),
                new ThirdValueObjectFake(2, 3.5, "AAC"),
                true
            ),
            "All members different"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (new ThirdValueObjectFake(2, 3.1, "AAB"), null, true),
            "Right side is null"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (null, new ThirdValueObjectFake(2, 3.1, "AAB"), true),
            "Left side is null"
        );
        yield return TestCase.Of<ValueObjectOperatorCase>(
            (null, null, false),
            "Both sides are null"
        );
    }

    public static IEnumerable<Func<TestDataRow<ValueObjectHashCodeCase>>> GetHashCodeTestData()
    {
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new FirstValueObjectFake(1, "AA"), new FirstValueObjectFake(1, "AA"), true),
            "Same members"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new FirstValueObjectFake(2, "AAB"), new FirstValueObjectFake(2, "AAB"), true),
            "Same members (2, AAB)"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new FirstValueObjectFake(2, null), new FirstValueObjectFake(2, null), true),
            "Same members with null"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                true
            ),
            "Same three members"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new ThirdValueObjectFake(2, 3, "AABC"), new ThirdValueObjectFake(2, 3, "AABC"), true),
            "Same three members (2, 3, AABC)"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new FirstValueObjectFake(1, "AA"), new FirstValueObjectFake(1, "AC"), false),
            "Different second member"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new FirstValueObjectFake(3, "AAB"), new FirstValueObjectFake(2, "AAB"), false),
            "Different first member"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new FirstValueObjectFake(2, null), new FirstValueObjectFake(2, "AS"), false),
            "Null vs non-null second member"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (
                new ThirdValueObjectFake(1, 3.2, "AAB"),
                new ThirdValueObjectFake(1, 3.1, "AAB"),
                false
            ),
            "Different second member (three-member)"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new ThirdValueObjectFake(2, 3, "AABC"), new ThirdValueObjectFake(2, 33, "AAB"), false),
            "Different second and third members"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new ThirdValueObjectFake(2, 3, "AABC"), new FirstValueObjectFake(2, "AAB"), false),
            "Different value object types"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new FirstValueObjectFake(2, null), new ThirdValueObjectFake(2, 3, "AABC"), false),
            "Different value object types (First vs Third)"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new FirstValueObjectFake(2, null), "1", false),
            "Value object compared with raw string"
        );
        yield return TestCase.Of<ValueObjectHashCodeCase>(
            (new ThirdValueObjectFake(2, 3, "AABC"), 2, false),
            "Value object compared with raw int"
        );
    }

    [Test]
    [MethodDataSource(nameof(EqualsTestData))]
    public async Task TestEquals_WhenValueObjectGiven_ThenMembersAreComapred(
        object lhsValueObject,
        object? rhsValueObject,
        bool expectedEqualsResult
    ) => await Assert.That(lhsValueObject.Equals(rhsValueObject)).IsEqualTo(expectedEqualsResult);

    [Test]
    [MethodDataSource(nameof(EqualsOperatorTestData))]
    public async Task TestEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred(
        ThirdValueObjectFake? lhsValueObject,
        ThirdValueObjectFake? rhsValueObject,
        bool expectedEqualsResult
    ) => await Assert.That(lhsValueObject != rhsValueObject).IsNotEqualTo(expectedEqualsResult);

    [Test]
    [MethodDataSource(nameof(NotEqualsOperatorTestData))]
    public async Task TestNotEqualsOperator_WhenValueObjectGiven_ThenMembersAreComapred(
        ThirdValueObjectFake? lhsValueObject,
        ThirdValueObjectFake? rhsValueObject,
        bool expectedEqualsResult
    ) => await Assert.That(lhsValueObject != rhsValueObject).IsEqualTo(expectedEqualsResult);

    [Test]
    [MethodDataSource(nameof(GetHashCodeTestData))]
    public async Task TestGetHashCode_WhenTwoValueObjectHaveSameMembers_ThenSameHashCodesAreReturned(
        object lhsValueObject,
        object rhsObject,
        bool expectedEqualsHashCodeResult
    ) =>
        await Assert
            .That(lhsValueObject.GetHashCode() == rhsObject.GetHashCode())
            .IsEqualTo(expectedEqualsHashCodeResult);
}
