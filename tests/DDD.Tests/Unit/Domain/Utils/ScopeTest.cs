using System;
using System.Linq;
using System.Threading.Tasks;
using DDD.Domain.Utils;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Utils;

[TestFixture]
public class ScopeTest
{
    [TearDown]
    public void ClearEventManager()
    {
        ScopeHandlerFake.CurrentScope = null;
        ScopeHandlerFake.Instance.Dispatcher = null;
    }

    [Test]
    public void TestConstructor_WhenCreating_ThenItemsListIsSetAsEmpty()
    {
        ScopeFake scope = new();

        Assert.That(scope.Items, Is.Empty);
    }

    [Test]
    public void TestConstructor_WhenCreating_ThenCurrentScopeFromScopeHandlerIsSetToCreatedScope()
    {
        ScopeFake scope = new();

        Assert.That(ScopeHandlerFake.CurrentScope, Is.SameAs(scope));
    }

    [Test]
    public void TestAdd_WhenNullItemGiven_ThenArgumentNullExceptionIsThrown()
    {
        using ScopeFake scope = new();
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("item"),
            () => scope.Add(null!)
        );
    }

    [Test]
    public void TestAdd_WhenItemGiven_ThenItemIsAdded()
    {
        string item = "item";
        ScopeFake? scope;
        using (scope = new())
        {
            scope.Add(item);

            Assert.That(scope.Items.AsEnumerable().Contains(item));
        }
    }

    [Test]
    public void TestApply_WhenApplingWithParentScope_ThenItemsAreAddedToParentScope()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock.Setup(e => e.Dispatch(It.IsAny<string>()));
        string item = "item";
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        using ScopeFake parentScopeFake = new();
        using (ScopeFake childScopeFake = new())
        {
            childScopeFake.Add(item);

            childScopeFake.Apply();
        }

        Assert.That(parentScopeFake.Items.Count, Is.EqualTo(1));
    }

    [Test]
    public void TestApply_WhenDispatcherIsUninitialized_ThenInvalidOperetionExceptionIsThrown()
    {
        string item = "item";

        using ScopeFake scope = new();
        scope.Add(item);

        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            scope.Apply
        );
    }

    [Test]
    public void TestApply_WhenMultipleNestedScopesGiven_ThenItemsAreAddedToParentScope()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock.Setup(e => e.Dispatch(It.IsAny<string>()));
        string item = "item";
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        using ScopeFake parentScopeFake = new();
        using (ScopeFake childScopeFake = new())
        {
            childScopeFake.Add(item);

            using (ScopeFake nestedChildScopeFake = new())
            {
                nestedChildScopeFake.Add(item);

                nestedChildScopeFake.Apply();
            }

            childScopeFake.Apply();
        }

        using (ScopeFake childScopeFake = new())
        {
            childScopeFake.Add(item);

            childScopeFake.Apply();
        }

        Assert.That(parentScopeFake.Items.Count, Is.EqualTo(3));
    }

    [Test]
    public void TestApply_WhenApplingWithoutParentScope_ThenItemsAreDispatched()
    {
        bool dispatched = false;
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;
        string item = "item";
        ScopeFake? scope = null;

        using (scope = new ScopeFake())
        {
            scope.Add(item);

            scope.Apply();
        }

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestApply_WhenAppling_ThenItemsAreCleared()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;
        string item = "item";
        ScopeFake? scope;
        using (scope = new ScopeFake())
        {
            scope.Add(item);

            scope.Apply();
        }

        Assert.That(scope.Items, Is.Empty);
    }

    [Test]
    public async Task TestApplyAsync_WhenApplingWithParentScope_ThenItemsAreAddedToParentScope()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock.Setup(e => e.DispatchAsync(It.IsAny<string>()));
        string item = "item";
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        using ScopeFake parentScopeFake = new();
        using (ScopeFake childScopeFake = new())
        {
            childScopeFake.Add(item);

            await childScopeFake.ApplyAsync();
        }

        Assert.That(parentScopeFake.Items.Count, Is.EqualTo(1));
    }

    [Test]
    public void TestApplyAsync_WhenDispatcherIsUninitialized_ThenInvalidOperetionExceptionIsThrown()
    {
        string item = "item";

        using ScopeFake scope = new();
        scope.Add(item);

        _ = Assert.ThrowsAsync(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            scope.ApplyAsync
        );
    }

    [Test]
    public async Task TestApplyAsync_WhenMultipleNestedScopesGiven_ThenItemsAreAddedToParentScope()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock.Setup(e => e.DispatchAsync(It.IsAny<string>()));
        string item = "item";
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        using ScopeFake parentScopeFake = new();
        using (ScopeFake childScopeFake = new())
        {
            childScopeFake.Add(item);

            using (ScopeFake nestedChildScopeFake = new())
            {
                nestedChildScopeFake.Add(item);

                await nestedChildScopeFake.ApplyAsync();
            }

            await childScopeFake.ApplyAsync();
        }

        using (ScopeFake childScopeFake = new())
        {
            childScopeFake.Add(item);

            await childScopeFake.ApplyAsync();
        }

        Assert.That(parentScopeFake.Items.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task TestApplyAsync_WhenApplingWithoutParentScope_ThenItemsAreDispatched()
    {
        bool dispatched = false;
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;
        string item = "item";
        ScopeFake? scope = null;

        using (scope = new ScopeFake())
        {
            scope.Add(item);

            await scope.ApplyAsync();
        }

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public async Task TestApplyAsync_WhenAppling_ThenItemsAreCleared()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;
        string item = "item";
        ScopeFake? scope;

        using (scope = new ScopeFake())
        {
            scope.Add(item);

            await scope.ApplyAsync();
        }

        Assert.That(scope.Items, Is.Empty);
    }

    [Test]
    public void TestClear_WhenClearing_ThenItemsAreEmpty()
    {
        string item = "item";
        ScopeFake? scope;

        using (scope = new ScopeFake())
        {
            scope.Add(item);

            scope.Clear();
        }

        Assert.That(scope.Items, Is.Empty);
    }

    [Test]
    public void TestDispose_WhenDisposingCurrentScope_ThenCurrentScopeIsNull()
    {
        using (ScopeFake scope = new()) { }

        Assert.That(ScopeHandlerFake.CurrentScope, Is.Null);
    }

    [Test]
    public void TestDispose_WhenParentScopeIsDisposedBeforeChildIsDisposed_ThenInvalidOperationExceptionIsThrown()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock.Setup(e => e.Dispatch(It.IsAny<string>()));
        string item = "item";
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        using ScopeFake parentScopeFake = new();
        using ScopeFake nestedChildScopeFake = new();
        nestedChildScopeFake.Add(item);

        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Scope nested incorrectly."),
            parentScopeFake.Dispose
        );
    }

    [Test]
    public void TestDispose_WhenParentScopeIsDisposedBeforeChildIsDisposed_ThenItemsAreCleared()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock.Setup(e => e.Dispatch(It.IsAny<string>()));
        string item = "item";
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;
        ScopeFake? parentScopeFake = null;

        try
        {
            using (parentScopeFake = new ScopeFake())
            {
                using ScopeFake nestedChildScopeFake = new();
                nestedChildScopeFake.Add(item);

                parentScopeFake.Dispose();
            }
        }
        catch (InvalidOperationException) { }

        Assert.That(parentScopeFake?.Items, Is.Empty);
    }
}
