using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

[NotInParallel]
internal class EventDispatcherTest
{
    private ServiceProvider ServiceProvider { get; set; }

    public EventDispatcherTest()
    {
        this.ServiceProvider = new ServiceCollection()
            .AddMediatR(cfg =>
            {
                _ = cfg.RegisterServicesFromAssembly(typeof(EventDispatcherTest).Assembly);
            })
            .BuildServiceProvider(false);
    }

    [Test]
    public async Task TestConstructing_WhenNullMediatorGiven_ThenArgumentNullExceptionIsThrown()
    {
        ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() =>
            new EventDispatcher(null!)
        );

        _ = await Assert.That(exception!.ParamName).IsEqualTo("mediator");
    }

    [After(Test)]
    public void ClearEventManager() => EventManager.Instance.Dispatcher = null;

    [Test]
    public async Task TestDispatch_WhenEventIsApplied_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        EventManager.Instance.UseMediatREventDispatcher(
            this.ServiceProvider.GetRequiredService<IMediator>()
        );

        EventManager.Instance.Notify(eventStub);

        _ = await Assert.That(eventStub.WasHandled).IsTrue();
    }

    [Test]
    public async Task TestDispatchAsync_WhenEventIsApplied_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        EventManager.Instance.UseMediatREventDispatcher(
            this.ServiceProvider.GetRequiredService<IMediator>()
        );

        await EventManager.Instance.NotifyAsync(eventStub);

        _ = await Assert.That(eventStub.WasHandled).IsTrue();
    }

    [Test]
    public async Task TestDispatch_WhenEventIsAppliedWithScope_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        EventManager.Instance.UseMediatREventDispatcher(
            this.ServiceProvider.GetRequiredService<IMediator>()
        );

        using EventsScope eventsScope = new();

        EventManager.Instance.Notify(eventStub);

        eventsScope.Publish();

        _ = await Assert.That(eventStub.WasHandled).IsTrue();
    }

    [Test]
    public async Task TestDispatchAsync_WhenEventIsAppliedWithScope_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        EventManager.Instance.UseMediatREventDispatcher(
            this.ServiceProvider.GetRequiredService<IMediator>()
        );

        using EventsScope eventsScope = new();

        await EventManager.Instance.NotifyAsync(eventStub);

        await eventsScope.PublishAsync();

        _ = await Assert.That(eventStub.WasHandled).IsTrue();
    }
}
