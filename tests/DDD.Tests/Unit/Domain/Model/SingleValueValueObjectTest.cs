using System;
using System.Collections.Generic;
using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Model;

[TestFixture]
public class SingleValueValueObjectTest
{
    public static IEnumerable<TestCaseData> EqualsTestData()
    {
        yield return new TestCaseData(
            new object[]
            {
                new SingleValueValueObjectFake("1"),
                new SingleValueValueObjectFake("1"),
                true
            }
        ).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(1)");
        yield return new TestCaseData(
            new object[]
            {
                new SingleValueValueObjectFake("3"),
                new SingleValueValueObjectFake("3"),
                true
            }
        ).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(2)");
        yield return new TestCaseData(
            new object[]
            {
                new SingleValueValueObjectFake("2"),
                new SingleValueValueObjectFake("2"),
                true
            }
        ).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(3)");
        yield return new TestCaseData(
            new object[]
            {
                new SingleValueValueObjectFake("1"),
                new SingleValueValueObjectFake("2"),
                false
            }
        ).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(4)");
        yield return new TestCaseData(
            new object[]
            {
                new SingleValueValueObjectFake("34"),
                new SingleValueValueObjectFake("3"),
                false
            }
        ).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(5)");
        yield return new TestCaseData(
            new object[]
            {
                new SingleValueValueObjectFake("5"),
                new SingleValueValueObjectFake("2"),
                false
            }
        ).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(6)");
        yield return new TestCaseData(
            new object?[] { new SingleValueValueObjectFake("5"), null, false }
        ).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(7)");
    }

    [Test]
    public void TestConstructing_WhenValueIsNotValid_ThenValidationExceptionsAreThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("value"),
            () => new SingleValueValueObjectFake(null!)
        );
    }

    [TestCaseSource(nameof(EqualsTestData))]
    public void TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared(
        SingleValueValueObjectFake lhsSingleValueValueObjectFake,
        SingleValueValueObjectFake rhsSingleValueValueObjectFake,
        bool expectedEqualityResult
    )
    {
        Assert.That(
            lhsSingleValueValueObjectFake.Equals(rhsSingleValueValueObjectFake),
            Is.EqualTo(expectedEqualityResult)
        );
    }

    [Test]
    public void TestEquals_WhenNullValueValueObjectsGiven_ThenReturnTrue()
    {
        Assert.That(
            new NullableSignleValueValueObjectFake(null),
            Is.EqualTo(new NullableSignleValueValueObjectFake(null))
        );
    }

    [Test]
    public void TestEquals_WhenOneNullValueValueObjectGiven_ThenReturnFalse()
    {
        Assert.That(
            new NullableSignleValueValueObjectFake("asd"),
            Is.Not.EqualTo(new NullableSignleValueValueObjectFake(null))
        );
    }

    [Test]
    public void ToString_WhenConverting_ThenValueIsReturned()
    {
        SingleValueValueObjectFake singleValueValueObjectFake = new("MyValue");

        string? stringValue = singleValueValueObjectFake.ToString();

        Assert.That(stringValue, Is.EqualTo("MyValue"));
    }

    [Test]
    public void ToString_WhenConvertingWithNullValue_ThenNullIsReturned()
    {
        NullableSignleValueValueObjectFake nullableSignleValueValueObjectFake = new(null);

        string? stringValue = nullableSignleValueValueObjectFake.ToString();

        Assert.That(stringValue, Is.Null);
    }
}
