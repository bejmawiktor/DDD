using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class SecondStringEnumerationFake : Enumeration<string?, SecondStringEnumerationFake>
{
    public static readonly SecondStringEnumerationFake? Null;
    public static readonly SecondStringEnumerationFake Zero = new(
        null
    );
    public static readonly SecondStringEnumerationFake One = new(
        nameof(One)
    );
    public static readonly SecondStringEnumerationFake Two = new(
        nameof(Two)
    );
    public static readonly SecondStringEnumerationFake Three = new(
        nameof(Three)
    );

    protected override string DefaultValue => nameof(One);

    public SecondStringEnumerationFake() { }

    protected SecondStringEnumerationFake(string? value)
        : base(value) { }
}