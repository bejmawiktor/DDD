using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class PropertyStringEnumerationFake : Enumeration<string, PropertyStringEnumerationFake>
{
    public static PropertyStringEnumerationFake One { get; } = new(nameof(One));
    public static PropertyStringEnumerationFake Two { get; } = new(nameof(Two));
    public static PropertyStringEnumerationFake Three { get; } = new(nameof(Three));

    public static IReadOnlyCollection<PropertyStringEnumerationFake> Favourites { get; } =
    [One, Three];

    protected override string DefaultValue => nameof(One);

    private static PropertyStringEnumerationFake Hidden { get; } = new(nameof(Hidden));

    public PropertyStringEnumerationFake() { }

    private PropertyStringEnumerationFake(string value)
        : base(value) { }
}
