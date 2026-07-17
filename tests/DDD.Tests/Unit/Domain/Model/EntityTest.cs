using DDD.Tests.Unit.Domain.TestDoubles;
using DDD.Tests.Unit.Utils;
using EntityEqualsCase = (object LhsEntity, object? RhsEntity, bool ExpectedEqualsResult);
using EntityOperatorCase = (
    DDD.Tests.Unit.Domain.TestDoubles.StringEntityStub? LhsEntity,
    DDD.Tests.Unit.Domain.TestDoubles.StringEntityStub? RhsEntity,
    bool ExpectedResult
);
using HashCodeCase = (object LhsEntity, object RhsEntity, bool ExpectedResult);

namespace DDD.Tests.Unit.Domain.Model;

public class EntityTest
{
    public static IEnumerable<Func<TestDataRow<EntityEqualsCase>>> ObjectEqualsTestData()
    {
        yield return TestCase.Of<EntityEqualsCase>(
            (new IntIdEntityStub(1), new IntIdEntityStub(1), true),
            "Same int ids"
        );
        yield return TestCase.Of<EntityEqualsCase>(
            (new IntIdEntityStub(123), new IntIdEntityStub(123), true),
            "Same int ids (123)"
        );
        yield return TestCase.Of<EntityEqualsCase>(
            (new StringEntityStub("1"), new StringEntityStub("1"), true),
            "Same string ids"
        );
        yield return TestCase.Of<EntityEqualsCase>(
            (new StringEntityStub("123"), new StringEntityStub("123"), true),
            "Same string ids (123)"
        );
        yield return TestCase.Of<EntityEqualsCase>(
            (new IntIdEntityStub(1), new IntIdEntityStub(2), false),
            "Different int ids"
        );
        yield return TestCase.Of<EntityEqualsCase>(
            (new IntIdEntityStub(1), new StringEntityStub("1"), false),
            "Different entity types with int and string id"
        );
        yield return TestCase.Of<EntityEqualsCase>(
            (new IntIdEntityStub(1), null, false),
            "Int entity compared with null"
        );
        yield return TestCase.Of<EntityEqualsCase>(
            (new StringEntityStub("1"), new StringEntityStub("12"), false),
            "Different string ids"
        );
        yield return TestCase.Of<EntityEqualsCase>(
            (new StringEntityStub("1"), null, false),
            "String entity compared with null"
        );
        yield return TestCase.Of<EntityEqualsCase>(
            (new StringEntityStub("1"), new OtherStringEntityStub("1"), false),
            "Different entity types with same string id"
        );
    }

    public static IEnumerable<Func<TestDataRow<EntityOperatorCase>>> EqualsOperatorTestData()
    {
        yield return TestCase.Of<EntityOperatorCase>(
            (new StringEntityStub("1"), new StringEntityStub("1"), true),
            "Same string ids"
        );
        yield return TestCase.Of<EntityOperatorCase>(
            (new StringEntityStub("123"), new StringEntityStub("123"), true),
            "Same string ids (123)"
        );
        yield return TestCase.Of<EntityOperatorCase>(
            (new StringEntityStub("1"), new StringEntityStub("12"), false),
            "Different string ids"
        );
        yield return TestCase.Of<EntityOperatorCase>(
            (new StringEntityStub("1"), null, false),
            "Right side is null"
        );
        yield return TestCase.Of<EntityOperatorCase>(
            (null, new StringEntityStub("1"), false),
            "Left side is null"
        );
        yield return TestCase.Of<EntityOperatorCase>((null, null, true), "Both sides are null");
    }

    public static IEnumerable<Func<TestDataRow<EntityOperatorCase>>> NotEqualsOperatorTestData()
    {
        yield return TestCase.Of<EntityOperatorCase>(
            (new StringEntityStub("1"), new StringEntityStub("1"), false),
            "Same string ids"
        );
        yield return TestCase.Of<EntityOperatorCase>(
            (new StringEntityStub("123"), new StringEntityStub("123"), false),
            "Same string ids (123)"
        );
        yield return TestCase.Of<EntityOperatorCase>(
            (new StringEntityStub("1"), new StringEntityStub("12"), true),
            "Different string ids"
        );
        yield return TestCase.Of<EntityOperatorCase>(
            (new StringEntityStub("1"), null, true),
            "Right side is null"
        );
        yield return TestCase.Of<EntityOperatorCase>(
            (null, new StringEntityStub("1"), true),
            "Left side is null"
        );
        yield return TestCase.Of<EntityOperatorCase>((null, null, false), "Both sides are null");
    }

    public static IEnumerable<Func<TestDataRow<HashCodeCase>>> GetHashCodeTestData()
    {
        yield return TestCase.Of<HashCodeCase>(
            (new IntIdEntityStub(1), new IntIdEntityStub(1), true),
            "Same int ids"
        );
        yield return TestCase.Of<HashCodeCase>(
            (new IntIdEntityStub(123), new IntIdEntityStub(123), true),
            "Same int ids (123)"
        );
        yield return TestCase.Of<HashCodeCase>(
            (new StringEntityStub("1"), new StringEntityStub("1"), true),
            "Same string ids"
        );
        yield return TestCase.Of<HashCodeCase>(
            (new StringEntityStub("123"), new StringEntityStub("123"), true),
            "Same string ids (123)"
        );
        yield return TestCase.Of<HashCodeCase>(
            (new IntIdEntityStub(1), new IntIdEntityStub(2), false),
            "Different int ids"
        );
        yield return TestCase.Of<HashCodeCase>(
            (new IntIdEntityStub(1), new StringEntityStub("1"), false),
            "Different entity types"
        );
        yield return TestCase.Of<HashCodeCase>(
            (new StringEntityStub("1"), new StringEntityStub("12"), false),
            "Different string ids"
        );
        yield return TestCase.Of<HashCodeCase>(
            (new StringEntityStub("1"), "1", false),
            "Entity compared with raw string"
        );
        yield return TestCase.Of<HashCodeCase>(
            (new StringEntityStub("1"), 2, false),
            "Entity compared with raw int"
        );
    }

    [Test]
    public async Task TestConstructing_WhenNullIdGiven_ThenArgumentNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            new StringEntityStub(null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("value");
    }

    [Test]
    public async Task TestSet_WhenNullIdGiven_ThenArgumentNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            new StringEntityStub("AAA").Id = null!
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("value");
    }

    [Test]
    public async Task TestSet_WhenProperIdGiven_ThenIdIsSet()
    {
        StringEntityStub entity = new("AAA") { Id = "BBB" };

        _ = await Assert.That(entity.Id).IsEqualTo("BBB");
    }

    [Test]
    public async Task TestConstructing_WhenIdGiven_ThenIdIsSet()
    {
        StringEntityStub stringEntityStub = new("1");

        _ = await Assert.That(stringEntityStub.Id).IsEqualTo("1");
    }

    [Test]
    [MethodDataSource(nameof(ObjectEqualsTestData))]
    public async Task TestEquals_WhenEntityGiven_ThenIdsAreCompared(
        object lhsEntity,
        object? rhsEntity,
        bool expectedEqualsResult
    ) => await Assert.That(lhsEntity.Equals(rhsEntity)).IsEqualTo(expectedEqualsResult);

    [Test]
    [MethodDataSource(nameof(EqualsOperatorTestData))]
    public async Task TestEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared(
        StringEntityStub? lhsEntity,
        StringEntityStub? rhsEntity,
        bool expectedEqualsResult
    ) => await Assert.That(lhsEntity == rhsEntity).IsEqualTo(expectedEqualsResult);

    [Test]
    [MethodDataSource(nameof(NotEqualsOperatorTestData))]
    public async Task TestNotEqualsOperator_WhenEntitiesGiven_ThenIdsAreCompared(
        StringEntityStub? lhsEntity,
        StringEntityStub? rhsEntity,
        bool expectedEqualsResult
    ) => await Assert.That(lhsEntity != rhsEntity).IsEqualTo(expectedEqualsResult);

    [Test]
    [MethodDataSource(nameof(GetHashCodeTestData))]
    public async Task TestGetHashCode_WhenTwoEntitiesHaveSameIds_ThenSameHashCodesAreReturned(
        object lhsEntity,
        object rhsEntity,
        bool expectedEqualsHashCodeResult
    ) =>
        await Assert
            .That(lhsEntity.GetHashCode() == rhsEntity.GetHashCode())
            .IsEqualTo(expectedEqualsHashCodeResult);
}
