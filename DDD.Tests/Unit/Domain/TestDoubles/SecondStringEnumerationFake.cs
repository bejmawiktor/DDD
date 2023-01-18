using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class SecondStringEnumerationFake : Enumeration<string?, SecondStringEnumerationFake>
    {
        public static readonly SecondStringEnumerationFake? Null;
        public static readonly SecondStringEnumerationFake Zero = new SecondStringEnumerationFake(null);
        public static readonly SecondStringEnumerationFake One = new SecondStringEnumerationFake(nameof(One));
        public static readonly SecondStringEnumerationFake Two = new SecondStringEnumerationFake(nameof(Two));
        public static readonly SecondStringEnumerationFake Three = new SecondStringEnumerationFake(nameof(Three));

        protected override string DefaultValue => nameof(One);

        public SecondStringEnumerationFake()
        {
        }

        protected SecondStringEnumerationFake(string? value) : base(value)
        {
        }
    }
}