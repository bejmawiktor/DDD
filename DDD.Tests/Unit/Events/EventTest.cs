﻿using DDD.Events;
using Moq;
using NUnit.Framework;
using System;

namespace DDD.Tests.Unit.Events
{
    [TestFixture]
    public class EventTest
    {
        [Test]
        public void TestConstructing_WhenConstructing_ThenGuidIsSet()
        {
            var @event = new Mock<Event>().Object;

            Assert.Multiple(() =>
            {
                Assert.That(@event.Id, Is.Not.Null);
                Assert.That(@event.Id, Is.Not.EqualTo(Guid.Empty));
            });
        }

        [Test]
        public void TestConstructing_WhenConstructing_ThenCreatedAtIsSet()
        {
            var @event = new Mock<Event>().Object;

            Assert.That(@event.CreatedAt, Is.GreaterThan(DateTime.MinValue));
        }

        [Test]
        public void TestConstructing_WhenConstructingMultipleEvents_ThenIdHasDiffrentValues()
        {
            var firstEvent = new Mock<Event>().Object;
            var secondEvent = new Mock<Event>().Object;

            Assert.That(firstEvent.Id, Is.Not.EqualTo(secondEvent));
        }
    }
}