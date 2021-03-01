using DDD.Domain.Persistence;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Persistence
{
    [TestFixture]
    public class PaginationTest
    {
        [Test]
        public void TestConstructing_WhenPageGiven_ThenPageIsSet()
        {
            var pagination = new Pagination(10, 1000);

            Assert.That(pagination.Page, Is.EqualTo(10));
        }

        [Test]
        public void TestConstructing_WhenItemsPerPageGiven_ThenItemsPerPageIsSet()
        {
            var pagination = new Pagination(10, 1000);

            Assert.That(pagination.ItemsPerPage, Is.EqualTo(1000));
        }
    }
}