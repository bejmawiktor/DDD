using DDD.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DDD.Tests.Unit
{
    [TestFixture]
    public class EnumerationTest
    {
        public class TestEnumeration : Enumeration<string, TestEnumeration>
        {
            public static TestEnumeration Null;
            public static TestEnumeration Zero = new TestEnumeration(null);
            public static TestEnumeration One = new TestEnumeration(nameof(One));
            public static TestEnumeration Two = new TestEnumeration(nameof(Two));
            public static TestEnumeration Three = new TestEnumeration(nameof(Three));

            protected override string DefaultValue => nameof(One);

            public TestEnumeration()
            {
            }

            protected TestEnumeration(string value) : base(value)
            {
            }
        }

        public class TestEnumeration2 : Enumeration<string, TestEnumeration>
        {
            public static TestEnumeration2 Null;
            public static TestEnumeration2 Zero = new TestEnumeration2(null);
            public static TestEnumeration2 One = new TestEnumeration2(nameof(One));
            public static TestEnumeration2 Two = new TestEnumeration2(nameof(Two));
            public static TestEnumeration2 Three = new TestEnumeration2(nameof(Three));

            protected override string DefaultValue => nameof(One);

            public TestEnumeration2()
            {
            }

            protected TestEnumeration2(string value) : base(value)
            {
            }
        }

        public static IEnumerable<object[]> EqualityByEqualsTestData
        {
            get
            {
                yield return new object[]
                {
                    TestEnumeration.One,
                    TestEnumeration.One,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.Two,
                    TestEnumeration.Two,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.Three,
                    TestEnumeration.Three,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.Zero,
                    TestEnumeration.Zero,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.Zero,
                    null,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.One,
                    null,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Two,
                    TestEnumeration.Three,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Zero,
                    TestEnumeration.Three,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Three,
                    TestEnumeration.Zero,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Zero,
                    TestEnumeration2.Zero,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Three,
                    TestEnumeration2.Three,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Three,
                    TestEnumeration2.Null,
                    false
                };
            }
        }

        public static IEnumerable<object[]> EqualityByOperatorTestData
        {
            get
            {
                yield return new object[]
                {
                    TestEnumeration.One,
                    TestEnumeration.One,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.Two,
                    TestEnumeration.Two,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.Three,
                    TestEnumeration.Three,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.Zero,
                    TestEnumeration.Zero,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.Zero,
                    null,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.One,
                    null,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Two,
                    TestEnumeration.Three,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Zero,
                    TestEnumeration.Three,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Three,
                    TestEnumeration.Zero,
                    false
                };
            }
        }

        public static IEnumerable<object[]> GetHashCodeTestData
        {
            get
            {
                yield return new object[]
                {
                    TestEnumeration.Zero,
                    TestEnumeration.Zero,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.One,
                    TestEnumeration.One,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration.Two,
                    TestEnumeration.Two,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration2.Three,
                    TestEnumeration2.Three,
                    true
                };
                yield return new object[]
                {
                    TestEnumeration2.Three,
                    TestEnumeration2.Two,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.Three,
                    TestEnumeration2.Three,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.One,
                    TestEnumeration2.Three,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration.One,
                    TestEnumeration.Three,
                    false
                };
                yield return new object[]
                {
                    TestEnumeration2.One,
                    TestEnumeration2.Three,
                    false
                };
            }
        }

        [TestCaseSource(nameof(EqualityByEqualsTestData))]
        public void TestEqualityUsingEqualsMethod(
            object lhsEnumeration,
            object rhsEnumeration,
            bool expectedEqualsResult)
        {
            Assert.That(lhsEnumeration.Equals(rhsEnumeration), Is.EqualTo(expectedEqualsResult));
        }

        [Test]
        public void TestNullCoalescing()
        {
            Assert.That(TestEnumeration.CollateNull(null), Is.EqualTo(TestEnumeration.Default));
            Assert.That(TestEnumeration.CollateNull(TestEnumeration.One), Is.EqualTo(TestEnumeration.One));
        }

        [Test]
        public void TestGettingValues()
        {
            IEnumerable<TestEnumeration> expectedValues = new TestEnumeration[]
            {
                TestEnumeration.One,
                TestEnumeration.Two,
                TestEnumeration.Three,
                TestEnumeration.Zero,
                TestEnumeration.Null
            };

            Assert.That(TestEnumeration.GetValues(), Is.EquivalentTo(expectedValues));
        }

        [Test]
        public void TestGettingNames()
        {
            IEnumerable<string> expectedNames = new string[]
            {
                nameof(TestEnumeration.One),
                nameof(TestEnumeration.Two),
                nameof(TestEnumeration.Three),
                nameof(TestEnumeration.Zero),
                nameof(TestEnumeration.Null)
            };

            Assert.That(TestEnumeration.GetNames(), Is.EquivalentTo(expectedNames));
        }

        [TestCaseSource(nameof(EqualityByOperatorTestData))]
        public void TestEqualityUsingEqualsOperators(
            TestEnumeration lhsEnumeration,
            TestEnumeration rhsEnumeration,
            bool expectedEqualsResult)
        {
            Assert.That(lhsEnumeration == rhsEnumeration, Is.EqualTo(expectedEqualsResult));
            Assert.That(lhsEnumeration != rhsEnumeration, Is.Not.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(GetHashCodeTestData))]
        public void TestGettingHashCode(object lhsValueObject, object rhsObject, bool expectedEqualsHashCodeResult)
        {
            Assert.That(lhsValueObject.GetHashCode() == rhsObject.GetHashCode(), Is.EqualTo(expectedEqualsHashCodeResult));
        }

        [Test]
        public void TestConvertingFromValueWithWrongValue()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo($"Wrong {nameof(TestEnumeration)} value."),
                () => { TestEnumeration wrongValue = (TestEnumeration)"Test"; });
        }

        [Test]
        public void TestConvertingFromValue()
        {
            TestEnumeration twoValue = (TestEnumeration)nameof(TestEnumeration.Two);
            TestEnumeration nullValue = (TestEnumeration)((string)null);

            Assert.That(twoValue, Is.EqualTo(TestEnumeration.Two));
            Assert.That(nullValue, Is.EqualTo(TestEnumeration.Zero));
        }

        [Test]
        public void TestConvertingToValue()
        {
            string twoValue = TestEnumeration.Two;
            string zeroValue = TestEnumeration.Zero;
            string nullValue = TestEnumeration.Null;

            Assert.That(twoValue, Is.EqualTo(nameof(TestEnumeration.Two)));
            Assert.That(zeroValue, Is.EqualTo(null));
            Assert.That(nullValue, Is.EqualTo(null));
        }

        [Test]
        public void TestGettingDefaultValue()
        {
            TestEnumeration defaultValue = TestEnumeration.Default;

            Assert.That((string)defaultValue, Is.EqualTo(nameof(TestEnumeration.One)));
        }
    }
}