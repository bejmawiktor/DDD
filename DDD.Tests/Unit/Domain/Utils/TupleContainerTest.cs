using DDD.Domain.Utils;
using NUnit.Framework;
using System;

namespace DDD.Tests.Unit.Domain.Utils
{
    using DecimalTupleContainer = TupleContainer<Tuple<decimal>>;
    using FirstMultiMemberTupleContainer = TupleContainer<(string param1, decimal param2)>;
    using SecondMultiMemberTupleContainer = TupleContainer<(decimal param1, string param2)>;
    using StringTupleContainer = TupleContainer<Tuple<string>>;

    [TestFixture]
    public class TupleContainerTest
    {
        private readonly StringTupleContainer stringTupleContainer1 = new StringTupleContainer(new Tuple<string>("AA"));
        private readonly StringTupleContainer stringTupleContainer2 = new StringTupleContainer(new Tuple<string>("AB"));
        private readonly DecimalTupleContainer decimalTupleContainer1 = new DecimalTupleContainer(new Tuple<decimal>(1m));
        private readonly DecimalTupleContainer decimalTupleContainer2 = new DecimalTupleContainer(new Tuple<decimal>(2m));
        private readonly FirstMultiMemberTupleContainer firstMultiTupleContainer1 = new FirstMultiMemberTupleContainer(("AA", 1m));
        private readonly FirstMultiMemberTupleContainer firstMultiTupleContainer2 = new FirstMultiMemberTupleContainer(("AA", 2m));
        private readonly SecondMultiMemberTupleContainer secondMultiTupleContainer1 = new SecondMultiMemberTupleContainer((1m, "AA"));
        private readonly SecondMultiMemberTupleContainer secondMultiTupleContainer2 = new SecondMultiMemberTupleContainer((2m, "AA"));

        [Test]
        public void TestConstructing_WhenNullTupleGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tuple"),
                () => { TupleContainer<Tuple<string>> tuple = new TupleContainer<Tuple<string>>(null!); });
        }

        [Test]
        public void TestConstructing_WhenTupleGiven_ThenTupleIsSet()
        {
            var tupleContainer = new TupleContainer<(string name, decimal value)>(("AA", 22m));

            Assert.That(tupleContainer.Tuple, Is.EqualTo(("AA", 22m)));
        }

        [Test]
        public void TestLength_WhenTupleGiven_ThenLengthIsSameAsTuple()
        {
            var tupleContainer = new TupleContainer<(string name, decimal value)>(("AA", 22m));

            Assert.That(tupleContainer.Length, Is.EqualTo(2));
        }

        [Test]
        public void TestIndexerOperator_WhenIndexGiven_ThenTupleElementIsReturned()
        {
            var tupleContainer = new TupleContainer<(string name, decimal value)>(("AA", 22m));

            Assert.Multiple(() =>
            {
                Assert.That(tupleContainer[0], Is.EqualTo("AA"));
                Assert.That(tupleContainer[1], Is.EqualTo(22m));
            });
        }

        [Test]
        public void TestEquals_WhenTupleContainerGivenGiven_ThenTuplesAreCompared()
        {
            Assert.Multiple(() =>
            {
                Assert.That(this.stringTupleContainer1, Is.EqualTo(new StringTupleContainer(new Tuple<string>("AA"))));
                Assert.That(this.stringTupleContainer2, Is.EqualTo(new StringTupleContainer(new Tuple<string>("AB"))));
                Assert.That(this.decimalTupleContainer1, Is.EqualTo(new DecimalTupleContainer(new Tuple<decimal>(1m))));
                Assert.That(this.decimalTupleContainer2, Is.EqualTo(new DecimalTupleContainer(new Tuple<decimal>(2m))));
                Assert.That(this.firstMultiTupleContainer1, Is.EqualTo(new FirstMultiMemberTupleContainer(("AA", 1m))));
                Assert.That(this.firstMultiTupleContainer2, Is.EqualTo(new FirstMultiMemberTupleContainer(("AA", 2m))));
                Assert.That(this.secondMultiTupleContainer1, Is.EqualTo(new SecondMultiMemberTupleContainer((1m, "AA"))));
                Assert.That(this.secondMultiTupleContainer2, Is.EqualTo(new SecondMultiMemberTupleContainer((2m, "AA"))));
                Assert.That(this.stringTupleContainer1, Is.Not.EqualTo(this.stringTupleContainer2));
                Assert.That(this.stringTupleContainer2, Is.Not.EqualTo(this.stringTupleContainer1));
                Assert.That(this.decimalTupleContainer1, Is.Not.EqualTo(this.decimalTupleContainer2));
                Assert.That(this.stringTupleContainer2, Is.Not.EqualTo(this.decimalTupleContainer1));
                Assert.That(this.decimalTupleContainer1, Is.Not.EqualTo(this.decimalTupleContainer2));
                Assert.That(this.decimalTupleContainer1, Is.Not.EqualTo(this.stringTupleContainer1));
                Assert.That(this.firstMultiTupleContainer1, Is.Not.EqualTo(this.firstMultiTupleContainer2));
                Assert.That(this.firstMultiTupleContainer1, Is.Not.EqualTo(this.decimalTupleContainer1));
                Assert.That(this.secondMultiTupleContainer2, Is.Not.EqualTo(this.firstMultiTupleContainer2));
                Assert.That(this.stringTupleContainer1, Is.Not.EqualTo(null));
                Assert.That(null!, Is.Not.EqualTo(this.stringTupleContainer2));
                Assert.That((StringTupleContainer?)null, Is.EqualTo((StringTupleContainer?)null));
            });
        }

        [Test]
        public void TestEqualsOperator_WhenTupleContainerGivenGiven_ThenTuplesAreCompared()
        {
            Assert.Multiple(() =>
            {
                Assert.That(this.stringTupleContainer1 == new StringTupleContainer(new Tuple<string>("AA")), Is.True);
                Assert.That(this.stringTupleContainer2 == new StringTupleContainer(new Tuple<string>("AB")), Is.True);
                Assert.That(this.decimalTupleContainer1 == new DecimalTupleContainer(new Tuple<decimal>(1m)), Is.True);
                Assert.That(this.decimalTupleContainer2 == new DecimalTupleContainer(new Tuple<decimal>(2m)), Is.True);
                Assert.That(this.firstMultiTupleContainer1 == new FirstMultiMemberTupleContainer(("AA", 1m)), Is.True);
                Assert.That(this.firstMultiTupleContainer2 == new FirstMultiMemberTupleContainer(("AA", 2m)), Is.True);
                Assert.That(this.secondMultiTupleContainer1 == new SecondMultiMemberTupleContainer((1m, "AA")), Is.True);
                Assert.That(this.secondMultiTupleContainer2 == new SecondMultiMemberTupleContainer((2m, "AA")), Is.True);
                Assert.That(this.stringTupleContainer1 == this.stringTupleContainer2, Is.False);
                Assert.That(this.stringTupleContainer2 == this.stringTupleContainer1, Is.False);
                Assert.That(this.decimalTupleContainer1 == this.decimalTupleContainer2, Is.False);
                Assert.That(this.decimalTupleContainer1 == this.decimalTupleContainer2, Is.False);
                Assert.That(this.firstMultiTupleContainer1 == this.firstMultiTupleContainer2, Is.False);
                Assert.That(this.stringTupleContainer1 == null!, Is.False);
                Assert.That(null! == this.stringTupleContainer2, Is.False);
                Assert.That((StringTupleContainer?)null! == (StringTupleContainer?)null!, Is.True);
            });
        }

        [Test]
        public void TestNotEqualsOperator_WhenTupleContainerGivenGiven_ThenTuplesAreCompared()
        {
            Assert.Multiple(() =>
            {
                Assert.That(this.stringTupleContainer1 != new StringTupleContainer(new Tuple<string>("AA")), Is.False);
                Assert.That(this.stringTupleContainer2 != new StringTupleContainer(new Tuple<string>("AB")), Is.False);
                Assert.That(this.decimalTupleContainer1 != new DecimalTupleContainer(new Tuple<decimal>(1m)), Is.False);
                Assert.That(this.decimalTupleContainer2 != new DecimalTupleContainer(new Tuple<decimal>(2m)), Is.False);
                Assert.That(this.firstMultiTupleContainer1 != new FirstMultiMemberTupleContainer(("AA", 1m)), Is.False);
                Assert.That(this.firstMultiTupleContainer2 != new FirstMultiMemberTupleContainer(("AA", 2m)), Is.False);
                Assert.That(this.secondMultiTupleContainer1 != new SecondMultiMemberTupleContainer((1m, "AA")), Is.False);
                Assert.That(this.secondMultiTupleContainer2 != new SecondMultiMemberTupleContainer((2m, "AA")), Is.False);
                Assert.That(this.stringTupleContainer1 != this.stringTupleContainer2, Is.True);
                Assert.That(this.stringTupleContainer2 != this.stringTupleContainer1, Is.True);
                Assert.That(this.decimalTupleContainer1 != this.decimalTupleContainer2, Is.True);
                Assert.That(this.decimalTupleContainer1 != this.decimalTupleContainer2, Is.True);
                Assert.That(this.firstMultiTupleContainer1 != this.firstMultiTupleContainer2, Is.True);
                Assert.That(this.stringTupleContainer1 != null!, Is.True);
                Assert.That(null! != this.stringTupleContainer2, Is.True);
                Assert.That((StringTupleContainer?)null! != (StringTupleContainer?)null!, Is.False);
            });
        }

        [Test]
        public void TestGetHashCode_WhenTwoTupleContainersHaveSameTuples_ThenSameHashCodesAreReturned()
        {
            Assert.Multiple(() =>
            {
                Assert.That(this.stringTupleContainer1.GetHashCode(), Is.EqualTo(new StringTupleContainer(new Tuple<string>("AA")).GetHashCode()));
                Assert.That(this.stringTupleContainer2.GetHashCode(), Is.EqualTo(new StringTupleContainer(new Tuple<string>("AB")).GetHashCode()));
                Assert.That(this.decimalTupleContainer1.GetHashCode(), Is.EqualTo(new DecimalTupleContainer(new Tuple<decimal>(1m)).GetHashCode()));
                Assert.That(this.decimalTupleContainer2.GetHashCode(), Is.EqualTo(new DecimalTupleContainer(new Tuple<decimal>(2m)).GetHashCode()));
                Assert.That(this.firstMultiTupleContainer1.GetHashCode(), Is.EqualTo(new FirstMultiMemberTupleContainer(("AA", 1m)).GetHashCode()));
                Assert.That(this.firstMultiTupleContainer2.GetHashCode(), Is.EqualTo(new FirstMultiMemberTupleContainer(("AA", 2m)).GetHashCode()));
                Assert.That(this.secondMultiTupleContainer1.GetHashCode(), Is.EqualTo(new SecondMultiMemberTupleContainer((1m, "AA")).GetHashCode()));
                Assert.That(this.secondMultiTupleContainer2.GetHashCode(), Is.EqualTo(new SecondMultiMemberTupleContainer((2m, "AA")).GetHashCode()));
                Assert.That(this.stringTupleContainer1.GetHashCode(), Is.Not.EqualTo(this.stringTupleContainer2.GetHashCode()));
                Assert.That(this.stringTupleContainer2.GetHashCode(), Is.Not.EqualTo(this.stringTupleContainer1.GetHashCode()));
                Assert.That(this.decimalTupleContainer1.GetHashCode(), Is.Not.EqualTo(this.decimalTupleContainer2.GetHashCode()));
                Assert.That(this.stringTupleContainer2.GetHashCode(), Is.Not.EqualTo(this.decimalTupleContainer1.GetHashCode()));
                Assert.That(this.decimalTupleContainer1.GetHashCode(), Is.Not.EqualTo(this.decimalTupleContainer2.GetHashCode()));
                Assert.That(this.decimalTupleContainer1.GetHashCode(), Is.Not.EqualTo(this.stringTupleContainer1.GetHashCode()));
                Assert.That(this.firstMultiTupleContainer1.GetHashCode(), Is.Not.EqualTo(this.firstMultiTupleContainer2.GetHashCode()));
                Assert.That(this.firstMultiTupleContainer1.GetHashCode(), Is.Not.EqualTo(this.decimalTupleContainer1.GetHashCode()));
                Assert.That(this.secondMultiTupleContainer2.GetHashCode(), Is.Not.EqualTo(this.firstMultiTupleContainer2.GetHashCode()));
                Assert.That(this.stringTupleContainer1.GetHashCode(), Is.Not.EqualTo(null));
            });
        }

        [Test]
        public void TestCastingFromTupleContainerToTuple_WhenTupleContainerGiven_ThenTupleIsReturned()
        {
            (string param1, decimal param2) tuple = new FirstMultiMemberTupleContainer(("AA", 1m));

            Assert.That(tuple, Is.EqualTo(("AA", 1m)));
        }

        [Test]
        public void TestCastingFromTupleToTupleContainer_WhenTupleGiven_ThenTupleContainerIsReturned()
        {
            SecondMultiMemberTupleContainer tuple = (1m, "AA");

            Assert.That(tuple, Is.EqualTo(new SecondMultiMemberTupleContainer((1m, "AA"))));
        }
    }
}