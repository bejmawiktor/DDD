using System;
using BShelf.Core;
using DDD.Domain.Events;
using MediatR;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events.MediatR
{
    [TestFixture]
    internal class EventManagerExtensionTest
    {
        [TearDown]
        public void ClearEventManager()
        {
            EventManager.Instance.EventDispatcher = null;
        }

        [Test]
        public void TestUseMediatREventDispatcher_WhenMediatorGiven_ThenMediatorIsSet()
        {
            Mock<IMediator> mediatorMock = new Mock<IMediator>();
            IMediator mediator = mediatorMock.Object;

            EventManager.Instance.UseMediatREventDispatcher(mediator);

            Assert.That(EventManager.Instance.EventDispatcher, Is.Not.Null);
        }

        [Test]
        public void TestUseMediatREventDispatcher_WhenNullMediatorGiven_ThenNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("mediator"),
                () => EventManager.Instance.UseMediatREventDispatcher(null!)
            );
        }
    }
}
