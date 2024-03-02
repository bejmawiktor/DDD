using System;
using DDD.Domain.Events;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Events
{
    [TestFixture]
    public class EventTest
    {
        [Test]
        public void TestConstructing_WhenConstructing_ThenGuidIsSet()
        {
            Event @event = new Mock<Event>().Object;

            Assert.Multiple(() =>
            {
                Assert.That(@event.Id, Is.Not.Null);
                Assert.That(@event.Id, Is.Not.EqualTo(Guid.Empty));
            });
        }

        [Test]
        public void TestConstructing_WhenConstructing_ThenCreatedAtIsSet()
        {
            Event @event = new Mock<Event>().Object;

            Assert.That(@event.CreatedAt, Is.GreaterThan(DateTime.MinValue));
        }

        [Test]
        public void TestConstructing_WhenConstructingMultipleEvents_ThenIdHasDiffrentValues()
        {
            Event firstEvent = new Mock<Event>().Object;
            Event secondEvent = new Mock<Event>().Object;

            Assert.That(firstEvent.Id, Is.Not.EqualTo(secondEvent));
        }
    }
}
