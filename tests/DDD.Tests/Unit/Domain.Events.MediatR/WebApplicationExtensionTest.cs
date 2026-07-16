using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

[NotInParallel]
internal class WebApplicationExtensionTest
{
    [After(Test)]
    public void ClearEventManager() => EventManager.Instance.Dispatcher = null;

    [Test]
    public async Task TestUseMediatREventDispatcher_WhenUsed_ThenMediatRDispatcherIsUsedAsEventManagerDispatcher()
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

        await Assert.That(dispatchedEvent?.Event).IsSameReferenceAs(@event);
    }
}
