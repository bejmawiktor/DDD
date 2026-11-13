using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class MixedStringEnumerationFake : Enumeration<string, MixedStringEnumerationFake>
{
    public static readonly MixedStringEnumerationFake One = new(nameof(One));
    public static readonly MixedStringEnumerationFake Two = new(nameof(Two));

    public static MixedStringEnumerationFake Three { get; } = new(nameof(Three));
    public static MixedStringEnumerationFake Four { get; } = new(nameof(Four));

    protected override string DefaultValue => nameof(One);

    public MixedStringEnumerationFake() { }

    private MixedStringEnumerationFake(string value)
        : base(value) { }
}
