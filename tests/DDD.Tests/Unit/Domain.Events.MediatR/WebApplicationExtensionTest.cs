using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

internal class WebApplicationExtensionTest
{
    [TearDown]
    public void ClearEventManager() => EventManager.Instance.Dispatcher = null;

    [Test]
    public void TestUseMediatREventDispatcher_WhenUsed_ThenMediatRDispatcherIsUsedAsEventManagerDispatcher()
    {
        EventNotification<EventStub>? dispatchedEvent = null;
        EventStub @event = new();
        Mock<IMediator> mediatorMock = new();
        _ = mediatorMock
            .Setup(e => e.Publish(It.IsAny<INotification>(), default))
            .Returns(
                async (INotification notification, CancellationToken token) =>
                {
                    await Task.Run(() =>
                    {
                        dispatchedEvent = notification as EventNotification<EventStub>;
                    });
                }
            );
        WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder();
        _ = applicationBuilder.Services.AddSingleton(mediatorMock.Object);
        WebApplication application = applicationBuilder.Build();

        application.UseMediatREventDispatcher();

        EventManager.Instance.Notify(@event);

        Assert.That(dispatchedEvent?.Event, Is.SameAs(@event));
    }
}
