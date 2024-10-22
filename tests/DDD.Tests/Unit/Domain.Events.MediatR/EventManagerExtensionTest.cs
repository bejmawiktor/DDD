using System;
using DDD.Domain.Common;
using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using MediatR;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events.MediatR
{
    [TestFixture]
    internal class EventManagerExtensionTest
    {
        private IScopeHandler<EventsScope, IEvent, EventManager> EventManager =>
            DDD.Domain.Events.EventManager.Instance;

        [TearDown]
        public void ClearEventManager()
        {
            this.EventManager.Dispatcher = null;
        }

        [Test]
        public void TestUseMediatREventDispatcher_WhenMediatorGiven_ThenMediatorIsSet()
        {
            Mock<IMediator> mediatorMock = new();
            IMediator mediator = mediatorMock.Object;

            DDD.Domain.Events.EventManager.Instance.UseMediatREventDispatcher(mediator);

            Assert.That(this.EventManager.Dispatcher, Is.Not.Null);
        }

        [Test]
        public void TestUseMediatREventDispatcher_WhenNullMediatorGiven_ThenNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("mediator"),
                () => DDD.Domain.Events.EventManager.Instance.UseMediatREventDispatcher(null!)
            );
        }
    }
}
