using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace DDD.Tests.Unit.Domain.Events.MediatR
{
    [TestFixture]
    internal class EventDispatcherTest
    {
        [Test]
        public void TestConstructing_WhenNullMediatorGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("mediator"),
                () => new EventDispatcher(null!));
        }

        [TearDown]
        public void ClearEventManager()
        {
            EventManager.Instance.EventDispatcher = null;
        }

        [Test]
        public void TestDispatch_WhenEventIsPublished_ThenEventIsHandled()
        {
            EventStub eventStub = new();
            var servicesProvider = new ServiceCollection()
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(EventDispatcherTest).Assembly))
                .BuildServiceProvider();
            EventDispatcher eventDispatcher = new EventDispatcher(servicesProvider.GetRequiredService<IMediator>());
            EventManager.Instance.EventDispatcher = eventDispatcher;

            EventManager.Instance.Notify(eventStub);

            Assert.That(eventStub.WasHandled, Is.True);
        }
    }
}