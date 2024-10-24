using DDD.Domain.Common;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Domain.Common;

internal class ScopeHandlerTest
{
    private IScopeHandler<ScopeFake, string, ScopeHandlerFake> ScopeHandler =>
        ScopeHandlerFake.Instance;

    [TearDown]
    public void ClearScopeHandler()
    {
        ScopeHandlerFake.CurrentScope = null;
        this.ScopeHandler.Dispatcher = null;
    }

    [Test]
    public void TestScopeHandler_WhenNoScopeCreated_ThenItemsAreDispatchedImmediately()
    {
        bool dispatched = false;
        Mock<IDispatcher> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        this.ScopeHandler.Dispatcher = dispatcherMock.Object;

        this.ScopeHandler.Notify("item");

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestNotify_WhenScopeWasCreated_ThenItemsArentDispatched()
    {
        bool dispatched = false;
        string item = "item";
        Mock<IDispatcher> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        this.ScopeHandler.Dispatcher = dispatcherMock.Object;

        using(ScopeFake scope = new())
        {
            this.ScopeHandler.Notify(item);
        }

        Assert.That(dispatched, Is.False);
    }

    [Test]
    public void TestNotify_WhenScopeWasCreated_ThenItemsAreAddedToScopeItems()
    {
        Mock<IDispatcher> dispatcherMock = new();
        this.ScopeHandler.Dispatcher = dispatcherMock.Object;
        string item = "item";

        using ScopeFake scope = new();

        this.ScopeHandler.Notify(item);

        Assert.That(scope.Items.AsEnumerable().Contains(item));
    }

    [Test]
    public void TestNotify_WhenScopeWasntCreated_ThenItemsAreDispatched()
    {
        bool dispatched = false;
        Mock<IDispatcher> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        this.ScopeHandler.Dispatcher = dispatcherMock.Object;

        this.ScopeHandler.Notify("item");

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestNotify_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            () => this.ScopeHandler.Notify("item")
        );
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasCreated_ThenItemsArentDispatched()
    {
        bool dispatched = false;
        string item = "item";
        Mock<IDispatcher> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        this.ScopeHandler.Dispatcher = dispatcherMock.Object;

        using(ScopeFake scope = new())
        {
            await this.ScopeHandler.NotifyAsync(item);
        }

        Assert.That(dispatched, Is.False);
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasCreated_ThenItemsAreAddedToScopeItems()
    {
        Mock<IDispatcher> dispatcherMock = new();
        this.ScopeHandler.Dispatcher = dispatcherMock.Object;
        string item = "item";
        using ScopeFake scope = new();
        await this.ScopeHandler.NotifyAsync(item);

        Assert.That(scope.Items.Contains(item));
    }

    [Test]
    public async Task TestNotifyAsync_WhenScopeWasntCreated_ThenItemsAreDispatched()
    {
        bool dispatched = false;
        Mock<IDispatcher> dispatcherMock = new();
        _ = dispatcherMock
            .Setup(e => e.DispatchAsync(It.IsAny<string>()))
            .Callback(() => dispatched = true);
        this.ScopeHandler.Dispatcher = dispatcherMock.Object;

        await this.ScopeHandler.NotifyAsync("item");

        Assert.That(dispatched, Is.True);
    }

    [Test]
    public void TestNotifyAsync_WhenScopeWasntCreatedAndDispatcherIsUninitialized_ThenInvalidOperationExceptionIsThrown()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<InvalidOperationException>()
                .And.Message.EqualTo("Dispatcher is uninitialized."),
            async () => await this.ScopeHandler.NotifyAsync("item")
        );
    }
}