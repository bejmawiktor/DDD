using System;
using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using MediatR;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events.MediatR;

[TestFixture]
internal class EventManagerExtensionTest
{
    [TearDown]
    public void ClearEventManager() => EventManager.Instance.Dispatcher = null;

    [Test]
    public void TestUseMediatREventDispatcher_WhenMediatorGiven_ThenMediatorIsSet()
    {
        Mock<IMediator> mediatorMock = new();
        IMediator mediator = mediatorMock.Object;

        EventManager.Instance.UseMediatREventDispatcher(mediator);

        Assert.That(EventManager.Instance.Dispatcher, Is.Not.Null);
    }

    [Test]
    public void TestUseMediatREventDispatcher_WhenNullMediatorGiven_ThenNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("mediator"),
            () => DDD.Domain.Events.EventManager.Instance.UseMediatREventDispatcher(null!)
        );
    }
}
