using DDD.Domain.Events;
using DDD.Domain.Events.AspNetCore;
using DDD.Tests.Unit.Domain.TestDoubles;
using Microsoft.AspNetCore.Builder;
using Moq;

namespace DDD.Tests.Unit.Domain.Events.AspNetCore;

[NotInParallel]
internal class WebApplicationExtensionTest
{
    [After(Test)]
    public void ClearEventManager() => EventManager.Instance.Dispatcher = null;

    [Test]
    public async Task TestUseEventCompositeDispatcher_WhenConfigurationGiven_ThenMultipleDispatchersAreUsedInEventManager()
    {
        WebApplication application = WebApplication.CreateBuilder().Build();
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    firstDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    secondDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    thirdDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        application.UseCompositeEventDispatcher(configuration =>
            configuration
                .WithDispatcher(firstEventDispatcherMock.Object)
                .WithDispatcher(secondEventDispatcherMock.Object)
                .WithDispatcher(thirdEventDispatcherMock.Object)
        );

        EventManager.Instance.Notify(@event);

        using (Assert.Multiple())
        {
            await Assert.That(firstDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(secondDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(thirdDispatchedEvent).IsSameReferenceAs(@event);
        }
    }

    [Test]
    public async Task TestUseEventCompositeDispatcher_WhenConfigurationWithServiceProviderGiven_ThenMultipleDispatchersAreUsedInEventManager()
    {
        WebApplication application = WebApplication.CreateBuilder().Build();
        EventStub? firstDispatchedEvent = null;
        EventStub? secondDispatchedEvent = null;
        EventStub? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IEventDispatcher> firstEventDispatcherMock = new();
        _ = firstEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    firstDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> secondEventDispatcherMock = new();
        _ = secondEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    secondDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        Mock<IEventDispatcher> thirdEventDispatcherMock = new();
        _ = thirdEventDispatcherMock
            .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
            .Callback(
                (IEvent dispatchedEvent) =>
                {
                    thirdDispatchedEvent = dispatchedEvent as EventStub;
                }
            );
        application.UseCompositeEventDispatcher(
            (configuration, serviceProvider) =>
                configuration
                    .WithDispatcher(firstEventDispatcherMock.Object)
                    .WithDispatcher(secondEventDispatcherMock.Object)
                    .WithDispatcher(thirdEventDispatcherMock.Object)
        );

        EventManager.Instance.Notify(@event);

        using (Assert.Multiple())
        {
            await Assert.That(firstDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(secondDispatchedEvent).IsSameReferenceAs(@event);
            await Assert.That(thirdDispatchedEvent).IsSameReferenceAs(@event);
        }
    }
}
