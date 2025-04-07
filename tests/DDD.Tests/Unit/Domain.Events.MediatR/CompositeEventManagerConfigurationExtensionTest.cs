using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using MediatR;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

[TestFixture]
public class CompositeEventManagerConfigurationExtensionTest
{
    [TearDown]
    public void ClearEventManager() => DDD.Domain.Events.EventManager.Instance.Dispatcher = null;

    [Test]
    public void TestWithMediatRDispatcher_WhenMultipleDispatchersAdded_ThenMultipleDispatchAreExecuted()
    {
        EventNotification<EventStub>? firstDispatchedEvent = null;
        EventNotification<EventStub>? secondDispatchedEvent = null;
        EventNotification<EventStub>? thirdDispatchedEvent = null;
        EventStub @event = new();
        Mock<IMediator> firstEventMediatorMock = new();
        firstEventMediatorMock
            .Setup(e => e.Publish(It.IsAny<INotification>(), default))
            .Returns(
                async (INotification notification, CancellationToken token) =>
                {
                    await Task.Run(() =>
                    {
                        firstDispatchedEvent = notification as EventNotification<EventStub>;
                    });
                }
            );
        Mock<IMediator> secondEventMediatorMock = new();
        secondEventMediatorMock
            .Setup(e => e.Publish(It.IsAny<INotification>(), default))
            .Returns(
                async (INotification notification, CancellationToken token) =>
                {
                    await Task.Run(() =>
                    {
                        secondDispatchedEvent = notification as EventNotification<EventStub>;
                    });
                }
            );
        Mock<IMediator> thirdEventMediatorMock = new();
        thirdEventMediatorMock
            .Setup(e => e.Publish(It.IsAny<INotification>(), default))
            .Returns(
                async (INotification notification, CancellationToken token) =>
                {
                    await Task.Run(() =>
                    {
                        thirdDispatchedEvent = notification as EventNotification<EventStub>;
                    });
                }
            );
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configuration
                .WithMediatRDispatcher(firstEventMediatorMock.Object)
                .WithMediatRDispatcher(secondEventMediatorMock.Object)
                .WithMediatRDispatcher(thirdEventMediatorMock.Object)
        );

        EventManager.Instance.Notify(@event);

        Assert.Multiple(() =>
        {
            Assert.That(firstDispatchedEvent?.Event, Is.SameAs(@event));
            Assert.That(secondDispatchedEvent?.Event, Is.SameAs(@event));
            Assert.That(thirdDispatchedEvent?.Event, Is.SameAs(@event));
        });
    }

    [Test]
    public void TestWithMediatRDispatcher_WhenNullMediatorGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("mediator"),
            () => new CompositeEventDispatcherConfiguration().WithMediatRDispatcher(null!)
        );
    }
}
