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

        public class TestValueObject1a : ValueObject
        {
            public int Field1 { get; }
            public string Field2 { get; }

            public TestValueObject1a(int field1, string field2)
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

        public static IEnumerable<object[]> EqualityByEqualsTestData
        {
            get
            {
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(1, "AA"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject1(2, "AAB"),
                    new TestValueObject1(2, "AAB"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject1(2, null),
                    new TestValueObject1(2, null),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 3.1, "AAB"),
                    new TestValueObject2(1, 3.1, "AAB"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject2(2, 3, "AABC"),
                    new TestValueObject2(2, 3, "AABC"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 3.1, null),
                    new TestValueObject2(1, 3.1, null),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject3(1, 3.1, null, true),
                    new TestValueObject3(1, 3.1, null, true),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(2, "AA"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    null,
                    false
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(1, "AB"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(2, null),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject2(1, 0, "AA"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject2(2, 0, "AA"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject2(1, 0, "AB"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject2(2, 1, null),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject2(1, 0, null),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, null),
                    new TestValueObject2(1, 0, "AA"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 0, "AA"),
                    new TestValueObject1(1, "AA"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject3(1, 0, "AA", false),
                    new TestValueObject3(1, 0, "AA", true),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject3(1, 0, "AA", true),
                    new TestValueObject3(1, 0, "AA", false),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1a(1, "AA"),
                    false
                };
            }
        }

        public static IEnumerable<object[]> EqualityByOperatorsTestData
        {
            get
            {
                yield return new object[]
                {
                    new TestValueObject2(1, 3.1, "AAB"),
                    new TestValueObject2(1, 3.1, "AAB"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject2(2, 3.4, "AAC"),
                    new TestValueObject2(2, 3.4, "AAC"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject2(2, 3.5, "AAC"),
                    new TestValueObject2(2, 3.5, "AAC"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject2(2, 3.1, "AAB"),
                    new TestValueObject2(2, 3.1, "AAB"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 3.1, "AAB"),
                    new TestValueObject2(1, 3.2, "AAB"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(3, 3.5, "AAC"),
                    new TestValueObject2(2, 3.4, "AAC"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(3, 3.4, "AAD"),
                    new TestValueObject2(2, 3.5, "AAC"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(2, 3.1, "AAB"),
                    null,
                    false
                };
                yield return new object[]
                {
                    null,
                    new TestValueObject2(2, 3.1, "AAB"),
                    false
                };
                yield return new object[]
                {
                    null,
                    null,
                    true
                };
            }
        }

        public static IEnumerable<object[]> GetHashCodeTestData
        {
            get
            {
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(1, "AA"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject1(2, "AAB"),
                    new TestValueObject1(2, "AAB"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject1(2, null),
                    new TestValueObject1(2, null),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 3.1, "AAB"),
                    new TestValueObject2(1, 3.1, "AAB"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject2(2, 3, "AABC"),
                    new TestValueObject2(2, 3, "AABC"),
                    true
                };
                yield return new object[]
                {
                    new TestValueObject1(1, "AA"),
                    new TestValueObject1(1, "AC"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject1(3, "AAB"),
                    new TestValueObject1(2, "AAB"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject1(2, null),
                    new TestValueObject1(2, "AS"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(1, 3.2, "AAB"),
                    new TestValueObject2(1, 3.1, "AAB"),
                    false
                };
                yield return new object[]
                {
                    new TestValueObject2(2, 3, "AABC"),
                    new TestValueObject2(2, 33, "AAB"),
                    false
                };
            }
        }

        [TestCaseSource(nameof(EqualityByEqualsTestData))]
        public void TestEqualityUsingEqualsMethod(
            object lhsValueObject,
            object rhsValueObject,
            bool expectedEqualsResult)
        {
            Assert.That(lhsValueObject.Equals(rhsValueObject), Is.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(EqualityByOperatorsTestData))]
        public void TestEqualityUsingEqualsOperators(
            TestValueObject2 lhsValueObject,
            TestValueObject2 rhsValueObject,
            bool expectedEqualsResult)
        {
            Assert.That(lhsValueObject == rhsValueObject, Is.EqualTo(expectedEqualsResult));
            Assert.That(lhsValueObject != rhsValueObject, Is.Not.EqualTo(expectedEqualsResult));
        }

        [TestCaseSource(nameof(GetHashCodeTestData))]
        public void TestHashCodeGetting(object lhsEntity, object rhsEntity, bool expectedEqualsHashCodeResult)
        {
            Assert.That(lhsEntity.GetHashCode() == rhsEntity.GetHashCode(), Is.EqualTo(expectedEqualsHashCodeResult));
        }
    }
}