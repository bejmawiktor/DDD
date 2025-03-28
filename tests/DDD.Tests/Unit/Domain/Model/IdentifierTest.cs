using System;
using System.Collections.Generic;
using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Model;

[TestFixture]
public class IdentifierTest
{
    public static IEnumerable<TestCaseData> EqualsTestData()
    {
        yield return new TestCaseData(
            new object[] { new StringIdFake("1"), new StringIdFake("1"), true }
        ).SetName($"{nameof(TestEquals_WhenIdentifierGiven_ThenValuesAreCompared)}(1)");
        yield return new TestCaseData(
            new object[] { new StringIdFake("3"), new StringIdFake("3"), true }
        ).SetName($"{nameof(TestEquals_WhenIdentifierGiven_ThenValuesAreCompared)}(2)");
        yield return new TestCaseData(
            new object[] { new StringIdFake("2"), new StringIdFake("2"), true }
        ).SetName($"{nameof(TestEquals_WhenIdentifierGiven_ThenValuesAreCompared)}(3)");
        yield return new TestCaseData(
            new object[] { new StringIdFake("1"), new StringIdFake("2"), false }
        ).SetName($"{nameof(TestEquals_WhenIdentifierGiven_ThenValuesAreCompared)}(4)");
        yield return new TestCaseData(
            new object[] { new StringIdFake("34"), new StringIdFake("3"), false }
        ).SetName($"{nameof(TestEquals_WhenIdentifierGiven_ThenValuesAreCompared)}(5)");
        yield return new TestCaseData(
            new object[] { new StringIdFake("5"), new StringIdFake("2"), false }
        ).SetName($"{nameof(TestEquals_WhenIdentifierGiven_ThenValuesAreCompared)}(6)");
        yield return new TestCaseData(new object?[] { new StringIdFake("5"), null, false }).SetName(
            $"{nameof(TestEquals_WhenIdentifierGiven_ThenValuesAreCompared)}(7)"
        );
    }

    [Test]
    public void TestConstructing_WhenNullValueGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("value"),
            () => new StringIdFake(null!)
        );
    }

    [Test]
    public void TestConstructing_WhenNotValidValueGiven_ThenExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>().And.Message.EqualTo("Id could not be empty."),
            () => new StringIdFake("")
        );
    }

    [Test]
    public void TestConstructing_WhenValidValueGiven_ThenValueIsSet()
    {
        StringIdFake id = new("1");

        Assert.That(id.Value, Is.EqualTo("1"));
    }

    [Test]
    public void TestConstructing_WhenValidNotNullableValueGiven_ThenValueIsSet()
    {
        IntIdFake id = new(1);

        Assert.That(id.Value, Is.EqualTo(1));
    }

    [TestCaseSource(nameof(EqualsTestData))]
    public void TestEquals_WhenIdentifierGiven_ThenValuesAreCompared(
        StringIdFake lhsIdentifier,
        StringIdFake rhsIdentifier,
        bool expectedEqualityResult
    ) => Assert.That(lhsIdentifier.Equals(rhsIdentifier), Is.EqualTo(expectedEqualityResult));

    [Test]
    public void TestToString_WhenValueGiven_ThenConvertedStringValueIsReturned()
    {
        IntIdFake id = new(1000);

        Assert.That(id.ToString(), Is.EqualTo("1000"));
    }
}
