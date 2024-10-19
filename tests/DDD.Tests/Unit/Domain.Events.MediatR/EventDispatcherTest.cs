using System;
using System.Threading.Tasks;
using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

[TestFixture]
internal class EventDispatcherTest
{
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
    public void ClearEventManager() => EventManager.Instance.EventDispatcher = null;

    [Test]
    public void TestDispatch_WhenEventIsPublished_ThenEventIsHandled()
    {
        EventStub eventStub = new();
        ServiceProvider servicesProvider = new ServiceCollection()
            .AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(EventDispatcherTest).Assembly)
            )
            .BuildServiceProvider();
        EventDispatcher eventDispatcher = new(servicesProvider.GetRequiredService<IMediator>());
        EventManager.Instance.EventDispatcher = eventDispatcher;

        EventManager.Instance.Notify(eventStub);

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
        EventManager.Instance.EventDispatcher = eventDispatcher;

        await EventManager.Instance.NotifyAsync(eventStub);

        Assert.That(eventStub.WasHandled, Is.True);
    }
}
