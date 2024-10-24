using DDD.Domain.Common;
using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

[TestFixture]
internal class EventDispatcherTest
{
    private IScopeHandler<EventsScope, IEvent, EventManager> EventManager =>
        DDD.Domain.Events.EventManager.Instance;

    [Test]
    public void TestConstructing_WhenNullMediatorGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("mediator"),
            () => new EventDispatcher(null!)
        );
    }

    [TearDown]
    public void ClearEventManager() => DDD.Domain.Events.EventManager.Instance.Dispatcher = null;

    [Test]
    public void TestDispatch_WhenEventIsPublished_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        ServiceProvider servicesProvider = new ServiceCollection()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<EventStubHandler>())
            .BuildServiceProvider();
        EventDispatcher eventDispatcher = new(servicesProvider.GetRequiredService<IMediator>());
        this.EventManager.Dispatcher = eventDispatcher;

        this.EventManager.Notify(eventStub);

        Assert.That(eventStub.WasHandled, Is.True);
    }

    [Test]
    public async Task TestDispatchAsync_WhenEventIsPublished_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        ServiceProvider servicesProvider = new ServiceCollection()
            .AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(EventDispatcherTest).Assembly)
            )
            .BuildServiceProvider();
        EventDispatcher eventDispatcher = new(servicesProvider.GetRequiredService<IMediator>());
        this.EventManager.Dispatcher = eventDispatcher;

        await this.EventManager.NotifyAsync(eventStub);

        Assert.That(eventStub.WasHandled, Is.True);
    }
}