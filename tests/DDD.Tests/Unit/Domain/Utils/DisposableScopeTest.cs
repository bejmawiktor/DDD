using System;
using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Utils;

[TestFixture]
internal class DisposableScopeTest
{
    [Test]
    public void TestClear_WhenClearing_ThenItemsAreEmpty()
    {
        DisposableScopeFake? scope;

        using (scope = new DisposableScopeFake())
        {
            scope.Clear();
        }

        Assert.That(scope.Text, Is.Empty);
    }

    [Test]
    public void TestDispose_WhenDisposingCurrentScope_ThenCurrentScopeIsNull()
    {
        DisposableScopeFake scope = new();
        scope.Dispose();

        Assert.That(ScopeHandlerFake.CurrentScope, Is.Null);
        Assert.That(scope.Text, Is.Empty);
    }

    [Test]
    public void TestDispose_WhenParentScopeIsDisposedBeforeChildIsDisposed_ThenInvalidOperationExceptionIsThrown()
    {
        using DisposableScopeFake parentDisposableScopeFake = new();
        using DisposableScopeFake nestedChildDisposableScopeFake = new();

        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Scope nested incorrectly."),
            parentDisposableScopeFake.Dispose
        );
    }

    [Test]
    public void TestDispose_WhenParentScopeIsDisposedBeforeChildIsDisposed_ThenItemsAreCleared()
    {
        DisposableScopeFake? parentDisposableScopeFake = null;
        bool wasExceptionThrown = false;

        try
        {
            using (parentDisposableScopeFake = new DisposableScopeFake())
            {
                using DisposableScopeFake nestedChildDisposableScopeFake = new();

                parentDisposableScopeFake.Dispose();
            }
        }
        catch (InvalidOperationException)
        {
            wasExceptionThrown = true;
        }

        Assert.That(ScopeHandlerFake.CurrentScope, Is.Null);
        Assert.That(parentDisposableScopeFake?.Text, Is.Empty);
        Assert.That(wasExceptionThrown, Is.True);
    }
}
