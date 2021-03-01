using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class SecondStringEnumerationFake : Enumeration<string, SecondStringEnumerationFake>
    {
        public readonly static SecondStringEnumerationFake Null;
        public readonly static SecondStringEnumerationFake Zero = new SecondStringEnumerationFake(null);
        public readonly static SecondStringEnumerationFake One = new SecondStringEnumerationFake(nameof(One));
        public readonly static SecondStringEnumerationFake Two = new SecondStringEnumerationFake(nameof(Two));
        public readonly static SecondStringEnumerationFake Three = new SecondStringEnumerationFake(nameof(Three));

        protected override string DefaultValue => nameof(One);

        public SecondStringEnumerationFake()
        {
        }

        protected SecondStringEnumerationFake(string value) : base(value)
        {
        }
    }
}