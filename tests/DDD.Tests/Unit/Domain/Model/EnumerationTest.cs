using DDD.Tests.Unit.Domain.TestDoubles;
using DDD.Tests.Unit.Utils;
using EnumerationEqualsCase = (
    object LhsEnumeration,
    object? RhsEnumeration,
    bool ExpectedEqualsResult
);
using EnumerationHashCodeCase = (object LhsValueObject, object RhsObject, bool ExpectedResult);
using EnumerationOperatorCase = (
    DDD.Tests.Unit.Domain.TestDoubles.FirstStringEnumerationFake? LhsEnumeration,
    DDD.Tests.Unit.Domain.TestDoubles.FirstStringEnumerationFake? RhsEnumeration,
    bool ExpectedResult
);

namespace DDD.Tests.Unit.Domain.Model;

public class EnumerationTest
{
    public static IEnumerable<Func<TestDataRow<EnumerationEqualsCase>>> EqualsTestData()
    {
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.One, FirstStringEnumerationFake.One, true),
            "Same value One"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Two, FirstStringEnumerationFake.Two, true),
            "Same value Two"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Three, FirstStringEnumerationFake.Three, true),
            "Same value Three"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Zero, FirstStringEnumerationFake.Zero, true),
            "Same value Zero"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Zero, null, false),
            "Zero compared with null"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.One, null, false),
            "One compared with null"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Two, FirstStringEnumerationFake.Three, false),
            "Different values Two and Three"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Zero, FirstStringEnumerationFake.Three, false),
            "Different values Zero and Three"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Three, FirstStringEnumerationFake.Zero, false),
            "Different values Three and Zero"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Zero, SecondStringEnumerationFake.Zero, false),
            "Same value but different enumeration types (Zero)"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Three, SecondStringEnumerationFake.Three, false),
            "Same value but different enumeration types (Three)"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (FirstStringEnumerationFake.Three, SecondStringEnumerationFake.Null, false),
            "Different enumeration types with null value"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (SecondStringEnumerationFake.Zero, SecondStringEnumerationFake.Zero, true),
            "Same value Zero in second enumeration"
        );
        yield return TestCase.Of<EnumerationEqualsCase>(
            (SecondStringEnumerationFake.Zero, SecondStringEnumerationFake.Three, false),
            "Different values in second enumeration"
        );
    }

    public static IEnumerable<Func<TestDataRow<EnumerationOperatorCase>>> EqualsOperatorTestData()
    {
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.One, FirstStringEnumerationFake.One, true),
            "Same value One"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Two, FirstStringEnumerationFake.Two, true),
            "Same value Two"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Three, FirstStringEnumerationFake.Three, true),
            "Same value Three"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Zero, FirstStringEnumerationFake.Zero, true),
            "Same value Zero"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Zero, null, false),
            "Zero compared with null"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.One, null, false),
            "One compared with null"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Two, FirstStringEnumerationFake.Three, false),
            "Different values Two and Three"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Zero, FirstStringEnumerationFake.Three, false),
            "Different values Zero and Three"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Three, FirstStringEnumerationFake.Zero, false),
            "Different values Three and Zero"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<EnumerationOperatorCase>>
    > NotEqualsOperatorTestData()
    {
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.One, FirstStringEnumerationFake.One, false),
            "Same value One"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Two, FirstStringEnumerationFake.Two, false),
            "Same value Two"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Three, FirstStringEnumerationFake.Three, false),
            "Same value Three"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Zero, FirstStringEnumerationFake.Zero, false),
            "Same value Zero"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Zero, null, true),
            "Zero compared with null"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.One, null, true),
            "One compared with null"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Two, FirstStringEnumerationFake.Three, true),
            "Different values Two and Three"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Zero, FirstStringEnumerationFake.Three, true),
            "Different values Zero and Three"
        );
        yield return TestCase.Of<EnumerationOperatorCase>(
            (FirstStringEnumerationFake.Three, FirstStringEnumerationFake.Zero, true),
            "Different values Three and Zero"
        );
    }

    public static IEnumerable<Func<TestDataRow<EnumerationHashCodeCase>>> GetHashCodeTestData()
    {
        yield return TestCase.Of<EnumerationHashCodeCase>(
            (FirstStringEnumerationFake.Zero, FirstStringEnumerationFake.Zero, true),
            "Same value Zero"
        );
        yield return TestCase.Of<EnumerationHashCodeCase>(
            (FirstStringEnumerationFake.One, FirstStringEnumerationFake.One, true),
            "Same value One"
        );
        yield return TestCase.Of<EnumerationHashCodeCase>(
            (FirstStringEnumerationFake.Two, FirstStringEnumerationFake.Two, true),
            "Same value Two"
        );
        yield return TestCase.Of<EnumerationHashCodeCase>(
            (SecondStringEnumerationFake.Three, SecondStringEnumerationFake.Three, true),
            "Same value Three in second enumeration"
        );
        yield return TestCase.Of<EnumerationHashCodeCase>(
            (SecondStringEnumerationFake.Three, SecondStringEnumerationFake.Two, false),
            "Different values in second enumeration"
        );
        yield return TestCase.Of<EnumerationHashCodeCase>(
            (FirstStringEnumerationFake.Three, SecondStringEnumerationFake.Three, false),
            "Same value but different enumeration types (Three)"
        );
        yield return TestCase.Of<EnumerationHashCodeCase>(
            (FirstStringEnumerationFake.One, SecondStringEnumerationFake.Three, false),
            "Different values and enumeration types"
        );
        yield return TestCase.Of<EnumerationHashCodeCase>(
            (FirstStringEnumerationFake.One, FirstStringEnumerationFake.Three, false),
            "Different values One and Three"
        );
        yield return TestCase.Of<EnumerationHashCodeCase>(
            (SecondStringEnumerationFake.One, SecondStringEnumerationFake.Three, false),
            "Different values One and Three in second enumeration"
        );
    }

    [Test]
    [MethodDataSource(nameof(EqualsTestData))]
    public async Task TestEquals_WhenEnumerationGiven_ThenValuesAreCompared(
        object lhsEnumeration,
        object? rhsEnumeration,
        bool expectedEqualsResult
    ) => await Assert.That(lhsEnumeration.Equals(rhsEnumeration)).IsEqualTo(expectedEqualsResult);

    [Test]
    public async Task TestCollateNull_WhenNullEnumerationGiven_ThenDefaultIsReturned() =>
        await Assert
            .That(FirstStringEnumerationFake.CollateNull(null))
            .IsEqualTo(FirstStringEnumerationFake.Default);

    [Test]
    public async Task TestCollateNull_WhenEnumerationGiven_ThenGivenEnumerationIsReturned() =>
        await Assert
            .That(FirstStringEnumerationFake.CollateNull(FirstStringEnumerationFake.One))
            .IsEqualTo(FirstStringEnumerationFake.One);

    [Test]
    public async Task TestGetValues_WhenGettingValues_ThenValuesAreReturned()
    {
        IEnumerable<FirstStringEnumerationFake?> expectedValues =
        [
            FirstStringEnumerationFake.One,
            FirstStringEnumerationFake.Two,
            FirstStringEnumerationFake.Three,
            FirstStringEnumerationFake.Zero,
            FirstStringEnumerationFake.Null,
        ];

        await Assert.That(FirstStringEnumerationFake.GetValues()).IsEquivalentTo(expectedValues);
    }

    [Test]
    public async Task TestGetNames_WhenGettingNames_ThenNamesOfEnumerationValuesAreReturned()
    {
        IEnumerable<string> expectedNames =
        [
            nameof(FirstStringEnumerationFake.One),
            nameof(FirstStringEnumerationFake.Two),
            nameof(FirstStringEnumerationFake.Three),
            nameof(FirstStringEnumerationFake.Zero),
            nameof(FirstStringEnumerationFake.Null),
        ];

        await Assert.That(FirstStringEnumerationFake.GetNames()).IsEquivalentTo(expectedNames);
    }

    [Test]
    [MethodDataSource(nameof(EqualsOperatorTestData))]
    public async Task TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared(
        FirstStringEnumerationFake? lhsEnumeration,
        FirstStringEnumerationFake? rhsEnumeration,
        bool expectedEqualsResult
    ) => await Assert.That(lhsEnumeration == rhsEnumeration).IsEqualTo(expectedEqualsResult);

    [Test]
    [MethodDataSource(nameof(NotEqualsOperatorTestData))]
    public async Task TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared(
        FirstStringEnumerationFake? lhsEnumeration,
        FirstStringEnumerationFake? rhsEnumeration,
        bool expectedEqualsResult
    ) => await Assert.That(lhsEnumeration != rhsEnumeration).IsEqualTo(expectedEqualsResult);

    [Test]
    [MethodDataSource(nameof(GetHashCodeTestData))]
    public async Task TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned(
        object lhsValueObject,
        object rhsObject,
        bool expectedEqualsHashCodeResult
    ) =>
        await Assert
            .That(lhsValueObject.GetHashCode() == rhsObject.GetHashCode())
            .IsEqualTo(expectedEqualsHashCodeResult);

    [Test]
    public async Task TestCastingFromValueToEnumeration_WhenNotRecognizedValueGiven_ThenArgumentExceptionIsThrown()
    {
        ArgumentException? exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = (FirstStringEnumerationFake)"Test";
        });

        await Assert
            .That(exception!.Message)
            .IsEqualTo($"Wrong {nameof(FirstStringEnumerationFake)} value.");
    }

    [Test]
    public async Task TestCastingFromValueToEnumeration_WhenRecognizedValueGiven_ThenEnumerationIsReturned()
    {
        FirstStringEnumerationFake twoValue = (FirstStringEnumerationFake)nameof(
            FirstStringEnumerationFake.Two
        );
        FirstStringEnumerationFake nullValue = (FirstStringEnumerationFake)(string?)null;

        using (Assert.Multiple())
        {
            await Assert.That(twoValue).IsEqualTo(FirstStringEnumerationFake.Two);
            await Assert.That(nullValue).IsEqualTo(FirstStringEnumerationFake.Zero);
        }
    }

    [Test]
    public async Task TestCastingFromEnumerationToValue_WhenEnumerationGiven_ThenValueIsReturned()
    {
        string? twoValue = FirstStringEnumerationFake.Two;
        string? zeroValue = FirstStringEnumerationFake.Zero;
        string? nullValue = FirstStringEnumerationFake.Null;

        using (Assert.Multiple())
        {
            await Assert.That(twoValue).IsEqualTo(nameof(FirstStringEnumerationFake.Two));
            await Assert.That(zeroValue).IsNull();
            await Assert.That(nullValue).IsNull();
        }
    }

    [Test]
    public async Task TestDefault_WhenGettingDefault_ThenDefaultValueIsReturned()
    {
        FirstStringEnumerationFake defaultValue = FirstStringEnumerationFake.Default;

        await Assert.That((string?)defaultValue).IsEqualTo(nameof(FirstStringEnumerationFake.One));
    }

    [Test]
    public async Task TestToString_WhenConvertingNullValue_ThenNullIsReturned() =>
        await Assert.That(FirstStringEnumerationFake.Zero.ToString()).IsNull();

    [Test]
    public async Task TestToString_WhenConvertingValue_ThenToStringOfValueIsReturned() =>
        await Assert.That(FirstStringEnumerationFake.Three.ToString()).IsEqualTo("Three");
}
