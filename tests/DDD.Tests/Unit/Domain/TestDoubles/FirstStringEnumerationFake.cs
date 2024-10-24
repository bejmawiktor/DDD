using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class FirstStringEnumerationFake : Enumeration<string?, FirstStringEnumerationFake>
{
    public static readonly FirstStringEnumerationFake? Null;
    public static readonly FirstStringEnumerationFake Zero = new(
        null
    );
    public static readonly FirstStringEnumerationFake One = new(
        nameof(One)
    );
    public static readonly FirstStringEnumerationFake Two = new(
        nameof(Two)
    );
    public static readonly FirstStringEnumerationFake Three = new(
        nameof(Three)
    );

    protected override string DefaultValue => nameof(One);

    public FirstStringEnumerationFake() { }

    protected FirstStringEnumerationFake(string? value)
        : base(value) { }
}