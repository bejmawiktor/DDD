using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DDD.Tests.Unit.Domain.Model
{
    [TestFixture]
    public class OneValueValueObjectTest
    {
        public static IEnumerable<TestCaseData> EqualsTestData()
        {
            yield return new TestCaseData(new object[]
            {
                new OneValueValueObjectFake("1"),
                new OneValueValueObjectFake("1"),
                true
            }).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(1)");
            yield return new TestCaseData(new object[]
            {
                new OneValueValueObjectFake("3"),
                new OneValueValueObjectFake("3"),
                true
            }).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(2)");
            yield return new TestCaseData(new object[]
            {
                new OneValueValueObjectFake("2"),
                new OneValueValueObjectFake("2"),
                true
            }).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(3)");
            yield return new TestCaseData(new object[]
            {
                new OneValueValueObjectFake("1"),
                new OneValueValueObjectFake("2"),
                false
            }).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(4)");
            yield return new TestCaseData(new object[]
            {
                new OneValueValueObjectFake("34"),
                new OneValueValueObjectFake("3"),
                false
            }).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(5)");
            yield return new TestCaseData(new object[]
            {
                new OneValueValueObjectFake("5"),
                new OneValueValueObjectFake("2"),
                false
            }).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(6)");
            yield return new TestCaseData(new object[]
            {
                new OneValueValueObjectFake("5"),
                null,
                false
            }).SetName($"{nameof(TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared)}(7)");
        }

        [Test]
        public void TestConstructing_WhenValueIsNotValid_ThenValidationExceptionsAreThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("value"),
                () => new OneValueValueObjectFake(null));
        }

        [TestCaseSource(nameof(EqualsTestData))]
        public void TestEquals_WhenValueObjectsGiven_ThenValuesAreCompared(
            OneValueValueObjectFake lhsOneMemberValueObjectFake,
            OneValueValueObjectFake rhsOneMemberValueObjectFake,
            bool expectedEqualityResult)
        {
            Assert.That(
                lhsOneMemberValueObjectFake.Equals(rhsOneMemberValueObjectFake),
                Is.EqualTo(expectedEqualityResult));
        }

        [Test]
        public void ToString_WhenConverting_ThenValueIsReturned()
        {
            var oneMemberValueObjectFake = new OneValueValueObjectFake("MyValue");

            string stringValue = oneMemberValueObjectFake.ToString();

            Assert.That(stringValue, Is.EqualTo("MyValue"));
        }
    }
}