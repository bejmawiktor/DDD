using DDD.Domain.Events.MediatR;
using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events.MediatR
{
    [TestFixture]
    internal class NotificationTest
    {
        [Test]
        public void TestConstructing_WhenEventGiven_ThenEventIsSet()
        {
            EventStub eventStub = new();
            Notification<EventStub> notification = new(eventStub);

            Assert.That(notification.Event, Is.EqualTo(eventStub));
        }
    }
}