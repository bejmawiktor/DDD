using DDD.Domain.Events;
using Moq;

namespace DDD.Tests.Unit.Domain.Events;

public class EventTest
{
    [Test]
    public async Task TestConstructing_WhenConstructing_ThenGuidIsSet()
    {
        Event @event = new Mock<Event>().Object;

        _ = await Assert.That(@event.Id).IsNotEqualTo(Guid.Empty);
    }

    [Test]
    public async Task TestConstructing_WhenConstructing_ThenCreatedAtIsSet()
    {
        Event @event = new Mock<Event>().Object;

        _ = await Assert
            .That(@event.CreatedAt)
            .IsEqualTo(DateTime.UtcNow)
            .Within(TimeSpan.FromMinutes(1));
    }

    [Test]
    public async Task TestConstructing_WhenConstructingMultipleEvents_ThenIdHasDiffrentValues()
    {
        Event firstEvent = new Mock<Event>().Object;
        Event secondEvent = new Mock<Event>().Object;

        _ = await Assert.That(firstEvent.Id).IsNotEqualTo(secondEvent.Id);
    }
}
