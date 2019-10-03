using DDD.Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace DDD.Tests.Unit
{
    public class ValueObjectTest
    {
        public class TestValueObject1 : ValueObject
        {
            public int Field1 { get; }
            public string Field2 { get; }

            public TestValueObject1(int field1, string field2)
            {
                this.Field1 = field1;
                this.Field2 = field2;
            }

            protected override IEnumerable<object> GetEqualityMembers()
            {
                yield return this.Field1;
                yield return this.Field2;
            }
        }

        public class TestValueObject2 : ValueObject
        {
            public int Field1 { get; }
            public double Field2 { get; }
            public string Field3 { get; }

            public TestValueObject2(int field1, double field2, string field3)
            {
                this.Field1 = field1;
                this.Field2 = field2;
                this.Field3 = field3;
            }

            protected override IEnumerable<object> GetEqualityMembers()
            {
                yield return this.Field1;
                yield return this.Field2;
                yield return this.Field3;
            }
        }

        public class TestValueObject3 : ValueObject
        {
            public int Field1 { get; }
            public double Field2 { get; }
            public string Field3 { get; }
            public bool Field4 { get; }

            public TestValueObject3(int field1, double field2, string field3, bool field4)
            {
                this.Field1 = field1;
                this.Field2 = field2;
                this.Field3 = field3;
                this.Field4 = field4;
            }

            protected override IEnumerable<object> GetEqualityMembers()
            {
                yield return this.Field1;
                yield return this.Field2;
                if(this.Field4)
                {
                    yield return this.Field3;
                }
            }
        }

        public static IEnumerable<object[]> EqualityTestData
        {
            get
            {
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(1, "AA")
                };
                yield return new object[]
                {
                    new TestValueObject1(2, "AAB"),
                    new TestValueObject1(2, "AAB")
                };
                yield return new object[]
                {
                    new TestValueObject1(2, null),
                    new TestValueObject1(2, null)
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 3.1, "AAB"),
                    new TestValueObject2(1, 3.1, "AAB")
                };
                yield return new object[]
                {
                    new TestValueObject2(2, 3, "AABC"),
                    new TestValueObject2(2, 3, "AABC")
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 3.1, null),
                    new TestValueObject2(1, 3.1, null)
                };
                yield return new object[]
                {
                    new TestValueObject3(1, 3.1, null, true),
                    new TestValueObject3(1, 3.1, null, true)
                };
            }
        }

        public static IEnumerable<object[]> InequalityTestData
        {
            get
            {
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(2, "AA")
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    null
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(1, "AB")
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(2, null)
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject2(1, 0, "AA")
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject2(2, 0, "AA")
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject2(1, 0, "AB")
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject2(2, 1, null)
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject2(1, 0, null)
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, null),
                    new TestValueObject2(1, 0, "AA")
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject1(1, "AA")
                };
                yield return new object[]
                {
                    new TestValueObject3(1, 0, "AA", false),
                    new TestValueObject3(1, 0, "AA", true)
                };
                yield return new object[]
                {
                    new TestValueObject3(1, 0, "AA", true),
                    new TestValueObject3(1, 0, "AA", false)
                };
            }
        }

        [TestCaseSource(nameof(EqualityTestData))]
        public void TestEqualityUsingEqualsMethod(ValueObject lhsValueObject, ValueObject rhsValueObject)
        {
            Assert.That(lhsValueObject.Equals(rhsValueObject), Is.True);
        }

        [TestCaseSource(nameof(InequalityTestData))]
        public void TestInequalityUsingEqualsMethod(ValueObject lhsValueObject, ValueObject rhsValueObject)
        {
            Assert.That(lhsValueObject.Equals(rhsValueObject), Is.False);
        }

        [Test]
        public void TestEqualityUsingEqualsOperator()
        {
            TestValueObject1 lhsTestValueObject1 = new TestValueObject1(1, "AA");
            TestValueObject1 rhsTestValueObject1 = new TestValueObject1(1, "AA");
            TestValueObject2 lhsTestValueObject2 = new TestValueObject2(1, 0, "AAB");
            TestValueObject2 rhsTestValueObject2 = new TestValueObject2(1, 0, "AAB");

            Assert.That(lhsTestValueObject1 == rhsTestValueObject1, Is.True);
            Assert.That((TestValueObject1)null == (TestValueObject1)null, Is.True);
            Assert.That(lhsTestValueObject2 == rhsTestValueObject2, Is.True);
            Assert.That((TestValueObject2)null == (TestValueObject2)null, Is.True);
        }

        [Test]
        public void TestInequalityUsingEqualsOperator()
        {
            TestValueObject1 lhsTestValueObject1 = new TestValueObject1(1, "AA");
            TestValueObject1 rhsTestValueObject1 = new TestValueObject1(2, "AB");
            TestValueObject2 lhsTestValueObject2 = new TestValueObject2(1, 0, "AAB");
            TestValueObject2 rhsTestValueObject2 = new TestValueObject2(2, 2, "AAB");

            Assert.That(lhsTestValueObject1 == rhsTestValueObject1, Is.False);
            Assert.That(null == rhsTestValueObject1, Is.False);
            Assert.That(lhsTestValueObject1 == null, Is.False);
            Assert.That(lhsTestValueObject2 == rhsTestValueObject2, Is.False);
            Assert.That(lhsTestValueObject2 == null, Is.False);
            Assert.That(null == rhsTestValueObject2, Is.False);
        }

        [Test]
        public void TestEqualityUsingNotEqualsOperator()
        {
            TestValueObject1 lhsTestValueObject1 = new TestValueObject1(1, "AA");
            TestValueObject1 rhsTestValueObject1 = new TestValueObject1(1, "AA");
            TestValueObject2 lhsTestValueObject2 = new TestValueObject2(1, 0, "AAB");
            TestValueObject2 rhsTestValueObject2 = new TestValueObject2(1, 0, "AAB");

            Assert.That(lhsTestValueObject1 != rhsTestValueObject1, Is.False);
            Assert.That((TestValueObject1)null != (TestValueObject1)null, Is.False);
            Assert.That(lhsTestValueObject2 != rhsTestValueObject2, Is.False);
            Assert.That((TestValueObject2)null != (TestValueObject2)null, Is.False);
        }

        [Test]
        public void TestInequalityUsingNotEqualsOperator()
        {
            TestValueObject1 lhsTestValueObject1 = new TestValueObject1(1, "AA");
            TestValueObject1 rhsTestValueObject1 = new TestValueObject1(2, "AB");
            TestValueObject2 lhsTestValueObject2 = new TestValueObject2(1, 0, "AAB");
            TestValueObject2 rhsTestValueObject2 = new TestValueObject2(2, 2, "AAB");

            Assert.That(lhsTestValueObject1 != rhsTestValueObject1, Is.True);
            Assert.That(null != rhsTestValueObject1, Is.True);
            Assert.That(lhsTestValueObject1 != null, Is.True);
            Assert.That(lhsTestValueObject2 != rhsTestValueObject2, Is.True);
            Assert.That(lhsTestValueObject2 != null, Is.True);
            Assert.That(null != rhsTestValueObject2, Is.True);
        }
    }
}