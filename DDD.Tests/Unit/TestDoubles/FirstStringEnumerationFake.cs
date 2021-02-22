using DDD.Model;

namespace DDD.Tests.Unit.TestDoubles
{
    public class FirstStringEnumerationFake : Enumeration<string, FirstStringEnumerationFake>
    {
        public readonly static FirstStringEnumerationFake Null;
        public readonly static FirstStringEnumerationFake Zero = new FirstStringEnumerationFake(null);
        public readonly static FirstStringEnumerationFake One = new FirstStringEnumerationFake(nameof(One));
        public readonly static FirstStringEnumerationFake Two = new FirstStringEnumerationFake(nameof(Two));
        public readonly static FirstStringEnumerationFake Three = new FirstStringEnumerationFake(nameof(Three));

        protected override string DefaultValue => nameof(One);

        public FirstStringEnumerationFake()
        {
        }

        protected FirstStringEnumerationFake(string value) : base(value)
        {
        }
    }
}