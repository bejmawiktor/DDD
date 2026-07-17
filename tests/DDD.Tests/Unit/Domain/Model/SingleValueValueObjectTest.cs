using DDD.Tests.Unit.Domain.TestDoubles;
using DDD.Tests.Unit.Utils;
using SingleValueEqualsCase = (
    DDD.Tests.Unit.Domain.TestDoubles.SingleValueValueObjectFake LhsSingleValueValueObjectFake,
    DDD.Tests.Unit.Domain.TestDoubles.SingleValueValueObjectFake? RhsSingleValueValueObjectFake,
    bool ExpectedEqualityResult
);

namespace DDD.Tests.Unit.Domain.Model;

public class SingleValueValueObjectTest
{
    public static IEnumerable<Func<TestDataRow<SingleValueEqualsCase>>> EqualsTestData()
    {
        yield return TestCase.Of<SingleValueEqualsCase>(
            (new SingleValueValueObjectFake("1"), new SingleValueValueObjectFake("1"), true),
            "Equal values"
        );
        yield return TestCase.Of<SingleValueEqualsCase>(
            (new SingleValueValueObjectFake("3"), new SingleValueValueObjectFake("3"), true),
            "Equal values (3)"
        );
        yield return TestCase.Of<SingleValueEqualsCase>(
            (new SingleValueValueObjectFake("2"), new SingleValueValueObjectFake("2"), true),
            "Equal values (2)"
        );
        yield return TestCase.Of<SingleValueEqualsCase>(
            (new SingleValueValueObjectFake("1"), new SingleValueValueObjectFake("2"), false),
            "Different values"
        );
        yield return TestCase.Of<SingleValueEqualsCase>(
            (new SingleValueValueObjectFake("34"), new SingleValueValueObjectFake("3"), false),
            "Different values with shared prefix"
        );
        yield return TestCase.Of<SingleValueEqualsCase>(
            (new SingleValueValueObjectFake("5"), new SingleValueValueObjectFake("2"), false),
            "Different values (5 vs 2)"
        );
        yield return TestCase.Of<SingleValueEqualsCase>(
            (new SingleValueValueObjectFake("5"), null, false),
            "Compared with null"
        );
    }

    [Test]
    public async Task TestConstructing_WhenValueIsNotValid_ThenValidationExceptionsAreThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            new SingleValueValueObjectFake(null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("value");
    }

    [Test]
    [MethodDataSource(nameof(EqualsTestData))]
    public async Task TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared(
        SingleValueValueObjectFake lhsSingleValueValueObjectFake,
        SingleValueValueObjectFake? rhsSingleValueValueObjectFake,
        bool expectedEqualityResult
    ) =>
        await Assert
            .That(lhsSingleValueValueObjectFake.Equals(rhsSingleValueValueObjectFake))
            .IsEqualTo(expectedEqualityResult);

    [Test]
    public async Task TestEquals_WhenNullValueValueObjectsGiven_ThenReturnTrue() =>
        await Assert
            .That(new NullableSignleValueValueObjectFake(null))
            .IsEqualTo(new NullableSignleValueValueObjectFake(null));

    [Test]
    public async Task TestEquals_WhenOneNullValueValueObjectGiven_ThenReturnFalse() =>
        await Assert
            .That(new NullableSignleValueValueObjectFake("asd"))
            .IsNotEqualTo(new NullableSignleValueValueObjectFake(null));

    [Test]
    public async Task ToString_WhenConverting_ThenValueIsReturned()
    {
        SingleValueValueObjectFake singleValueValueObjectFake = new("MyValue");

        string? stringValue = singleValueValueObjectFake.ToString();

        _ = await Assert.That(stringValue).IsEqualTo("MyValue");
    }

    [Test]
    public async Task ToString_WhenConvertingWithNullValue_ThenNullIsReturned()
    {
        NullableSignleValueValueObjectFake nullableSignleValueValueObjectFake = new(null);

        string? stringValue = nullableSignleValueValueObjectFake.ToString();

        _ = await Assert.That(stringValue).IsNull();
    }
}
