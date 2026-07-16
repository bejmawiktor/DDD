using DDD.Tests.Unit.Domain.TestDoubles;
using DDD.Tests.Unit.Utils;
using IdentifierEqualsCase = (
    DDD.Tests.Unit.Domain.TestDoubles.StringIdFake LhsIdentifier,
    DDD.Tests.Unit.Domain.TestDoubles.StringIdFake? RhsIdentifier,
    bool ExpectedEqualityResult
);

namespace DDD.Tests.Unit.Domain.Model;

public class IdentifierTest
{
    public static IEnumerable<Func<TestDataRow<IdentifierEqualsCase>>> EqualsTestData()
    {
        yield return TestCase.Of<IdentifierEqualsCase>(
            (new StringIdFake("1"), new StringIdFake("1"), true),
            "Equal ids"
        );
        yield return TestCase.Of<IdentifierEqualsCase>(
            (new StringIdFake("3"), new StringIdFake("3"), true),
            "Equal ids (3)"
        );
        yield return TestCase.Of<IdentifierEqualsCase>(
            (new StringIdFake("2"), new StringIdFake("2"), true),
            "Equal ids (2)"
        );
        yield return TestCase.Of<IdentifierEqualsCase>(
            (new StringIdFake("1"), new StringIdFake("2"), false),
            "Different ids"
        );
        yield return TestCase.Of<IdentifierEqualsCase>(
            (new StringIdFake("34"), new StringIdFake("3"), false),
            "Different ids with shared prefix"
        );
        yield return TestCase.Of<IdentifierEqualsCase>(
            (new StringIdFake("5"), new StringIdFake("2"), false),
            "Different ids (5 vs 2)"
        );
        yield return TestCase.Of<IdentifierEqualsCase>(
            (new StringIdFake("5"), null, false),
            "Compared with null"
        );
    }

    [Test]
    public async Task TestConstructing_WhenNullValueGiven_ThenArgumentNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(
            () => new StringIdFake(null!)
        );

        await Assert.That(exception!.ParamName).IsEqualTo("value");
    }

    [Test]
    public async Task TestConstructing_WhenNotValidValueGiven_ThenExceptionIsThrown()
    {
        ArgumentException? exception = Assert.Throws<ArgumentException>(() => new StringIdFake(""));

        await Assert.That(exception!.Message).IsEqualTo("Id could not be empty.");
    }

    [Test]
    public async Task TestConstructing_WhenValidValueGiven_ThenValueIsSet()
    {
        StringIdFake id = new("1");

        await Assert.That(id.Value).IsEqualTo("1");
    }

    [Test]
    public async Task TestConstructing_WhenValidNotNullableValueGiven_ThenValueIsSet()
    {
        IntIdFake id = new(1);

        await Assert.That(id.Value).IsEqualTo(1);
    }

    [Test]
    [MethodDataSource(nameof(EqualsTestData))]
    public async Task TestEquals_WhenIdentifierGiven_ThenValuesAreCompared(
        StringIdFake lhsIdentifier,
        StringIdFake? rhsIdentifier,
        bool expectedEqualityResult
    ) => await Assert.That(lhsIdentifier.Equals(rhsIdentifier)).IsEqualTo(expectedEqualityResult);

    [Test]
    public async Task TestToString_WhenValueGiven_ThenConvertedStringValueIsReturned()
    {
        IntIdFake id = new(1000);

        await Assert.That(id.ToString()).IsEqualTo("1000");
    }
}
