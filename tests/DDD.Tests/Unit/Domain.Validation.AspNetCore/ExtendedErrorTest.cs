using DDD.Domain.Validation.AspNetCore;
using DDD.Tests.Unit.Utils;
using ExtendedErrorCase = (
    string Message,
    System.Collections.Generic.IDictionary<string, object?> Extensions
);

namespace DDD.Tests.Unit.Domain.Validation.AspNetCore;

public class ExtendedErrorTest
{
    public static IEnumerable<Func<TestDataRow<ExtendedErrorCase>>> CreateExtendedErrorTestData()
    {
        yield return TestCase.Of<ExtendedErrorCase>(
            ("my error", new Dictionary<string, object?>()),
            "Without extensions"
        );
        yield return TestCase.Of<ExtendedErrorCase>(
            ("my error", new Dictionary<string, object?>() { { "errorCode", "E-001" } }),
            "Single extension"
        );
        yield return TestCase.Of<ExtendedErrorCase>(
            (
                "my error 2",
                new Dictionary<string, object?>()
                {
                    { "errorCode", "E-002" },
                    { "attempts", 3 },
                    { "retryable", true },
                }
            ),
            "Multiple extensions"
        );
        yield return TestCase.Of<ExtendedErrorCase>(
            ("my error 3", new Dictionary<string, object?>() { { "errorCode", null } }),
            "Extension with null value"
        );
    }

    [Test]
    [MethodDataSource(nameof(CreateExtendedErrorTestData))]
    public async Task TestConstructing_WhenMessageAndExtensionsGiven_ThenMessageIsSet(
        string message,
        IDictionary<string, object?> extensions
    )
    {
        ExtendedError error = new(message, extensions);

        _ = await Assert.That(error.Message).IsEqualTo(message);
    }

    [Test]
    [MethodDataSource(nameof(CreateExtendedErrorTestData))]
    public async Task TestConstructing_WhenMessageAndExtensionsGiven_ThenExtensionsAreSet(
        string message,
        IDictionary<string, object?> extensions
    )
    {
        ExtendedError error = new(message, extensions);

        _ = await Assert.That(error.Extensions).IsEquivalentTo(extensions);
    }

    [Test]
    public async Task TestConstructing_WhenExtensionsGiven_ThenGivenExtensionsInstanceIsKept()
    {
        Dictionary<string, object?> extensions = new() { { "errorCode", "E-001" } };

        ExtendedError error = new("my error", extensions);

        _ = await Assert.That(error.Extensions).IsSameReferenceAs(extensions);
    }

    [Test]
    public async Task TestConstructing_WhenNullMessageGiven_ThenArgumentNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            new ExtendedError(null!, new Dictionary<string, object?>())
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("message");
    }

    [Test]
    public async Task TestConstructing_WhenOnlyMessageGiven_ThenMessageIsSet()
    {
        ExtendedError error = new("my error");

        _ = await Assert.That(error.Message).IsEqualTo("my error");
    }

    [Test]
    public async Task TestConstructing_WhenOnlyMessageGiven_ThenExtensionsAreEmpty()
    {
        ExtendedError error = new("my error");

        _ = await Assert.That(error.Extensions).IsEmpty();
    }

    [Test]
    public async Task TestConstructing_WhenOnlyMessageGiven_ThenExtensionsCanBeFilledIn()
    {
        ExtendedError error = new("my error");

        error.Extensions["errorCode"] = "E-001";

        _ = await Assert
            .That(error.Extensions)
            .IsEquivalentTo(new Dictionary<string, object?>() { { "errorCode", "E-001" } });
    }

    [Test]
    public async Task TestConstructing_WhenNullExtensionsGiven_ThenArgumentNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            new ExtendedError("my error", null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("extensions");
    }

    [Test]
    public async Task TestConstructing_WhenEmptyMessageGiven_ThenArgumentExceptionIsThrown()
    {
        ArgumentException? exception = Assert.Throws<ArgumentException>(() =>
            new ExtendedError("", new Dictionary<string, object?>())
        );

        _ = await Assert.That(exception).IsNotNull();
    }

    [Test]
    public async Task TestToString_WhenMessageGiven_ThenMessageIsReturned()
    {
        ExtendedError error = new("my error", new Dictionary<string, object?>());

        _ = await Assert.That(error.ToString()).IsEqualTo("my error");
    }
}
