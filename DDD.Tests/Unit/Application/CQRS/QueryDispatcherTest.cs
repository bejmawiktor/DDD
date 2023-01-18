using DDD.Application.CQRS;
using DDD.Tests.Unit.Domain.TestDoubles;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Application.CQRS
{
    [TestFixture]
    public class QueryDispatcherTest
    {
        [Test]
        public void TestConstructing_WhenNullDependencyResolverGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("dependencyResolver"),
                () => new QueryDispatcher(null!));
        }

        [Test]
        public void TestDispatch_WhenNullQueryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(() => null!);
            var queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("query"),
                () => queryDispatcher.Dispatch<QueryStub, string>(null!));
        }

        [Test]
        public void TestDispatch_WhenNotResolvableQueryGiven_ThenQueryHandlerNotFoundExceptionIsThrown()
        {
            var dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(() => null!);
            var queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<QueryHandlerNotFoundException>(),
                () => queryDispatcher.Dispatch<QueryStub, string>(new QueryStub()));
        }

        [Test]
        public void TestDispatch_WhenQueryHandlerResolved_ThenQueryIsHandled()
        {
            var queryHandlerMock = new Mock<IQueryHandler<QueryStub, string>>();
            queryHandlerMock
                .Setup(q => q.Handle(It.IsAny<QueryStub>()))
                .Returns("QueryResult");
            var dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(queryHandlerMock.Object);
            var queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            string result = queryDispatcher.Dispatch<QueryStub, string>(new QueryStub());

            Assert.That(result, Is.EqualTo("QueryResult"));
        }

        [Test]
        public void TestDispatchAsync_WhenNullQueryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(() => null!);
            var queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("query"),
                () => queryDispatcher.DispatchAsync<QueryStub, string>(null!).Wait());
            Assert.Throws(
                Is.InstanceOf<QueryHandlerNotFoundException>(),
                () => queryDispatcher.DispatchAsync<QueryStub, string>(new QueryStub()).Wait());
        }

        [Test]
        public void TestDispatchAsync_WhenNotResolvableQueryGiven_ThenQueryHandlerNotFoundExceptionIsThrown()
        {
            var dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(() => null!);
            var queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<QueryHandlerNotFoundException>(),
                () => queryDispatcher.DispatchAsync<QueryStub, string>(new QueryStub()).Wait());
        }

        [Test]
        public async Task TestDispatchAsync_WhenQueryHandlerResolved_ThenQueryIsHandled()
        {
            var queryHandlerMock = new Mock<IAsyncQueryHandler<QueryStub, string>>();
            queryHandlerMock.Setup(q => q.HandleAsync(It.IsAny<QueryStub>()))
                .ReturnsAsync("QueryResult");
            var dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock
                .Setup(d => d.Resolve<IAsyncQueryHandler<QueryStub, string>>())
                .Returns(queryHandlerMock.Object);
            var queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            string result = await queryDispatcher.DispatchAsync<QueryStub, string>(new QueryStub());

            Assert.That(result, Is.EqualTo("QueryResult"));
        }
    }
}