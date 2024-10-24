using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DDD.Tests.Unit.Domain.Model;

[TestFixture]
public class EnumerationTest
{
    public static IEnumerable<TestCaseData> EqualsTestData
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.One,
                    FirstStringEnumerationFake.One,
                    true
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(1)");
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Two,
                    FirstStringEnumerationFake.Two,
                    true
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(2)");
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Three,
                    FirstStringEnumerationFake.Three,
                    true
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(3)");
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Zero,
                    FirstStringEnumerationFake.Zero,
                    true
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(4)");
            yield return new TestCaseData(
                new object?[] { FirstStringEnumerationFake.Zero, null, false }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(5)");
            yield return new TestCaseData(
                new object?[] { FirstStringEnumerationFake.One, null, false }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(6)");
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Two,
                    FirstStringEnumerationFake.Three,
                    false
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(7)");
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Zero,
                    FirstStringEnumerationFake.Three,
                    false
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(8)");
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Three,
                    FirstStringEnumerationFake.Zero,
                    false
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(9)");
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Zero,
                    SecondStringEnumerationFake.Zero,
                    false
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(10)");
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Three,
                    SecondStringEnumerationFake.Three,
                    false
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(11)");
            yield return new TestCaseData(
                new object?[]
                {
                    FirstStringEnumerationFake.Three,
                    SecondStringEnumerationFake.Null,
                    false
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(12)");
            yield return new TestCaseData(
                new object[]
                {
                    SecondStringEnumerationFake.Zero,
                    SecondStringEnumerationFake.Zero,
                    true
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(13)");
            yield return new TestCaseData(
                new object[]
                {
                    SecondStringEnumerationFake.Zero,
                    SecondStringEnumerationFake.Three,
                    false
                }
            ).SetName($"{nameof(TestEquals_WhenEnumerationGiven_ThenValuesAreCompared)}(14)");
        }
    }

    public static IEnumerable<TestCaseData> EqualsOperatorTestData
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.One,
                    FirstStringEnumerationFake.One,
                    true
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Two,
                    FirstStringEnumerationFake.Two,
                    true
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(2)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Three,
                    FirstStringEnumerationFake.Three,
                    true
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(3)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Zero,
                    FirstStringEnumerationFake.Zero,
                    true
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(4)"
            );
            yield return new TestCaseData(
                new object?[] { FirstStringEnumerationFake.Zero, null, false }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(5)"
            );
            yield return new TestCaseData(
                new object?[] { FirstStringEnumerationFake.One, null, false }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(6)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Two,
                    FirstStringEnumerationFake.Three,
                    false
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(7)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Zero,
                    FirstStringEnumerationFake.Three,
                    false
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(8)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Three,
                    FirstStringEnumerationFake.Zero,
                    false
                }
            ).SetName(
                $"{nameof(TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(9)"
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
                    FirstStringEnumerationFake.One,
                    FirstStringEnumerationFake.One,
                    false
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Two,
                    FirstStringEnumerationFake.Two,
                    false
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(2)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Three,
                    FirstStringEnumerationFake.Three,
                    false
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(3)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Zero,
                    FirstStringEnumerationFake.Zero,
                    false
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(4)"
            );
            yield return new TestCaseData(
                new object?[] { FirstStringEnumerationFake.Zero, null, true }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(5)"
            );
            yield return new TestCaseData(
                new object?[] { FirstStringEnumerationFake.One, null, true }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(6)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Two,
                    FirstStringEnumerationFake.Three,
                    true
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(7)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Zero,
                    FirstStringEnumerationFake.Three,
                    true
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(8)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Three,
                    FirstStringEnumerationFake.Zero,
                    true
                }
            ).SetName(
                $"{nameof(TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared)}(9)"
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
                    FirstStringEnumerationFake.Zero,
                    FirstStringEnumerationFake.Zero,
                    true
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.One,
                    FirstStringEnumerationFake.One,
                    true
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Two,
                    FirstStringEnumerationFake.Two,
                    true
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    SecondStringEnumerationFake.Three,
                    SecondStringEnumerationFake.Three,
                    true
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    SecondStringEnumerationFake.Three,
                    SecondStringEnumerationFake.Two,
                    false
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.Three,
                    SecondStringEnumerationFake.Three,
                    false
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.One,
                    SecondStringEnumerationFake.Three,
                    false
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    FirstStringEnumerationFake.One,
                    FirstStringEnumerationFake.Three,
                    false
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    SecondStringEnumerationFake.One,
                    SecondStringEnumerationFake.Three,
                    false
                }
            ).SetName(
                $"{nameof(TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned)}(1)"
            );
        }
    }

    [TestCaseSource(nameof(EqualsTestData))]
    public void TestEquals_WhenEnumerationGiven_ThenValuesAreCompared(
        object lhsEnumeration,
        object rhsEnumeration,
        bool expectedEqualsResult
    ) => Assert.That(lhsEnumeration.Equals(rhsEnumeration), Is.EqualTo(expectedEqualsResult));

    [Test]
    public void TestCollateNull_WhenNullEnumerationGiven_ThenDefaultIsReturned()
    {
        Assert.That(
            FirstStringEnumerationFake.CollateNull(null),
            Is.EqualTo(FirstStringEnumerationFake.Default)
        );
    }

    [Test]
    public void TestCollateNull_WhenEnumerationGiven_ThenGivenEnumerationIsReturned()
    {
        Assert.That(
            FirstStringEnumerationFake.CollateNull(FirstStringEnumerationFake.One),
            Is.EqualTo(FirstStringEnumerationFake.One)
        );
    }

    [Test]
    public void TestGetValues_WhenGettingValues_ThenValuesAreReturned()
    {
        IEnumerable<FirstStringEnumerationFake?> expectedValues =
            new FirstStringEnumerationFake?[]
            {
                FirstStringEnumerationFake.One,
                FirstStringEnumerationFake.Two,
                FirstStringEnumerationFake.Three,
                FirstStringEnumerationFake.Zero,
                FirstStringEnumerationFake.Null
            };

        Assert.That(FirstStringEnumerationFake.GetValues(), Is.EquivalentTo(expectedValues));
    }

    [Test]
    public void TestGetNames_WhenGettingNames_ThenNamesOfEnumerationValuesAreReturned()
    {
        IEnumerable<string> expectedNames = new string[]
        {
            nameof(FirstStringEnumerationFake.One),
            nameof(FirstStringEnumerationFake.Two),
            nameof(FirstStringEnumerationFake.Three),
            nameof(FirstStringEnumerationFake.Zero),
            nameof(FirstStringEnumerationFake.Null)
        };

        Assert.That(FirstStringEnumerationFake.GetNames(), Is.EquivalentTo(expectedNames));
    }

    [TestCaseSource(nameof(EqualsOperatorTestData))]
    public void TestEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared(
        FirstStringEnumerationFake lhsEnumeration,
        FirstStringEnumerationFake rhsEnumeration,
        bool expectedEqualsResult
    ) => Assert.That(lhsEnumeration == rhsEnumeration, Is.EqualTo(expectedEqualsResult));

    [TestCaseSource(nameof(NotEqualsOperatorTestData))]
    public void TestNotEqualsOperator_WhenEnumerationsGiven_ThenValuesAreCompared(
        FirstStringEnumerationFake lhsEnumeration,
        FirstStringEnumerationFake rhsEnumeration,
        bool expectedEqualsResult
    ) => Assert.That(lhsEnumeration != rhsEnumeration, Is.EqualTo(expectedEqualsResult));

    [TestCaseSource(nameof(GetHashCodeTestData))]
    public void TestGetHashCode_WhenTwoEntitiesHaveSameValues_ThenSameHashCodesAreReturned(
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

    [Test]
    public void TestCastingFromValueToEnumeration_WhenNotRecognizedValueGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Message.EqualTo($"Wrong {nameof(FirstStringEnumerationFake)} value."),
            () =>
            {
                FirstStringEnumerationFake wrongValue = (FirstStringEnumerationFake)"Test";
            }
        );
    }

    [Test]
    public void TestCastingFromValueToEnumeration_WhenRecognizedValueGiven_ThenEnumerationIsReturned()
    {
        FirstStringEnumerationFake twoValue = (FirstStringEnumerationFake)nameof(
            FirstStringEnumerationFake.Two
        );
        FirstStringEnumerationFake nullValue = (FirstStringEnumerationFake)(string?)null;

        Assert.Multiple(() =>
        {
            Assert.That(twoValue, Is.EqualTo(FirstStringEnumerationFake.Two));
            Assert.That(nullValue, Is.EqualTo(FirstStringEnumerationFake.Zero));
        });
    }

    [Test]
    public void TestCastingFromEnumerationToValue_WhenEnumerationGiven_ThenValueIsReturned()
    {
        string? twoValue = FirstStringEnumerationFake.Two;
        string? zeroValue = FirstStringEnumerationFake.Zero;
        string? nullValue = FirstStringEnumerationFake.Null;

        Assert.Multiple(() =>
        {
            Assert.That(twoValue, Is.EqualTo(nameof(FirstStringEnumerationFake.Two)));
            Assert.That(zeroValue, Is.EqualTo(null));
            Assert.That(nullValue, Is.EqualTo(null));
        });
    }

    [Test]
    public void TestDefault_WhenGettingDefault_ThenDefaultValueIsReturned()
    {
        FirstStringEnumerationFake defaultValue = FirstStringEnumerationFake.Default;

        Assert.That((string?)defaultValue, Is.EqualTo(nameof(FirstStringEnumerationFake.One)));
    }

    [Test]
    public void TestToString_WhenConvertingNullValue_ThenNullIsReturned() => Assert.That(FirstStringEnumerationFake.Zero.ToString(), Is.Null);

    [Test]
    public void TestToString_WhenConvertingValue_ThenToStringOfValueIsReturned() => Assert.That(FirstStringEnumerationFake.Three.ToString(), Is.EqualTo("Three"));
}