using System;
using System.Linq;
using System.Threading.Tasks;
using DDD.Domain.Utils;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Utils;

internal class ScopeHandlerTest
{
    [TearDown]
    public void ClearScopeHandler()
    {
        ScopeHandlerFake.CurrentScope = null;
        ScopeHandlerFake.Instance.Dispatcher = null;
    }

    [Test]
    public void TestScopeHandler_WhenNoScopeCreated_ThenItemsAreDispatchedImmediately()
    {
        bool dispatched = false;
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        ScopeHandlerFake.Instance.Handle("item");

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestHandle_WhenScopeWasCreated_ThenItemsArentDispatched()
    {
        bool dispatched = false;
        string item = "item";
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        using (ScopeFake scope = new())
        {
            ScopeHandlerFake.Instance.Handle(item);
        }

        Assert.That(dispatched, Is.False);
    }

    [Test]
    public void TestHandle_WhenScopeWasCreated_ThenItemsAreAddedToScopeItems()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;
        string item = "item";

        using ScopeFake scope = new();

        ScopeHandlerFake.Instance.Handle(item);

        Assert.That(scope.Items.AsEnumerable().Contains(item));
    }

    [Test]
    public void TestHandle_WhenScopeWasntCreated_ThenItemsAreDispatched()
    {
        bool dispatched = false;
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        ScopeHandlerFake.Instance.Handle("item");

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestHandle_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            () => ScopeHandlerFake.Instance.Handle("item")
        );
    }

    [Test]
    public async Task TestHandleAsync_WhenScopeWasCreated_ThenItemsArentDispatched()
    {
        bool dispatched = false;
        string item = "item";
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        using (ScopeFake scope = new())
        {
            await ScopeHandlerFake.Instance.HandleAsync(item);
        }

        Assert.That(dispatched, Is.False);
    }

    [Test]
    public async Task TestHandleAsync_WhenScopeWasCreated_ThenItemsAreAddedToScopeItems()
    {
        Mock<IDispatcher<string>> dispatcherMock = new();
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;
        string item = "item";
        using ScopeFake scope = new();
        await ScopeHandlerFake.Instance.HandleAsync(item);

        Assert.That(scope.Items.Contains(item));
    }

    [Test]
    public async Task TestHandleAsync_WhenScopeWasntCreated_ThenItemsAreDispatched()
    {
        bool dispatched = false;
        Mock<IDispatcher<string>> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        ScopeHandlerFake.Instance.Dispatcher = dispatcherMock.Object;

        await ScopeHandlerFake.Instance.HandleAsync("item");

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestHandleAsync_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            async () => await ScopeHandlerFake.Instance.HandleAsync("item")
        );
    }
}
