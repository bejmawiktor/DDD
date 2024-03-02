using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class FirstStringEnumerationFake : Enumeration<string?, FirstStringEnumerationFake>
    {
        public static readonly FirstStringEnumerationFake? Null;
        public static readonly FirstStringEnumerationFake Zero = new FirstStringEnumerationFake(
            null
        );
        public static readonly FirstStringEnumerationFake One = new FirstStringEnumerationFake(
            nameof(One)
        );
        public static readonly FirstStringEnumerationFake Two = new FirstStringEnumerationFake(
            nameof(Two)
        );
        public static readonly FirstStringEnumerationFake Three = new FirstStringEnumerationFake(
            nameof(Three)
        );

        protected override string DefaultValue => nameof(One);

        public FirstStringEnumerationFake() { }

        protected FirstStringEnumerationFake(string? value)
            : base(value) { }
    }
}
