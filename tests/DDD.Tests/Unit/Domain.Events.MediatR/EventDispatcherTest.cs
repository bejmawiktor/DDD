using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

internal class EventDispatcherTest
{
    private ServiceProvider ServiceProvider { get; set; }

    public EventDispatcherTest()
    {
        this.ServiceProvider = new ServiceCollection()
            .AddMediatR(cfg =>
            {
                _ = cfg.RegisterServicesFromAssembly(typeof(EventDispatcherTest).Assembly);
            }
            )
            .BuildServiceProvider(false);
    }

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
    public void TestDispatch_WhenEventIsApplied_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        DDD.Domain.Events.EventManager.Instance.UseMediatREventDispatcher(this.ServiceProvider.GetRequiredService<IMediator>());

        EventManager.Instance.Notify(eventStub);

        Assert.That(eventStub.WasHandled, Is.True);
    }

    [Test]
    public async Task TestDispatchAsync_WhenEventIsApplied_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        DDD.Domain.Events.EventManager.Instance.UseMediatREventDispatcher(this.ServiceProvider.GetRequiredService<IMediator>());

        await EventManager.Instance.NotifyAsync(eventStub);

        Assert.That(eventStub.WasHandled, Is.True);
    }


    [Test]
    public void TestDispatch_WhenEventIsAppliedWithScope_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        DDD.Domain.Events.EventManager.Instance.UseMediatREventDispatcher(this.ServiceProvider.GetRequiredService<IMediator>());
        
        using EventsScope eventsScope = new();
        
        EventManager.Instance.Notify(eventStub);

        eventsScope.Publish();

        Assert.That(eventStub.WasHandled, Is.True);
    }

    [Test]
    public async Task TestDispatchAsync_WhenEventIsAppliedWithScope_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        DDD.Domain.Events.EventManager.Instance.UseMediatREventDispatcher(this.ServiceProvider.GetRequiredService<IMediator>());
        
        using EventsScope eventsScope = new();

        await EventManager.Instance.NotifyAsync(eventStub);

        await eventsScope.PublishAsync();

        Assert.That(eventStub.WasHandled, Is.True);
    }
}
