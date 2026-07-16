namespace DDD.Tests.Unit.Utils;

public static class TestCase
{
    public static Func<TestDataRow<TData>> Of<TData>(TData data, string testName) =>
        () => new TestDataRow<TData>(data, testName);
}
