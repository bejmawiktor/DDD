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
            Mock<IDependencyResolver> dependencyResolverMock = new();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(() => null!);
            QueryDispatcher queryDispatcher = new(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("query"),
                () => queryDispatcher.Dispatch<QueryStub, string>(null!));
        }

        [Test]
        public void TestDispatch_WhenNotResolvableQueryGiven_ThenQueryHandlerNotFoundExceptionIsThrown()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(() => null!);
            QueryDispatcher queryDispatcher = new(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<QueryHandlerNotFoundException>(),
                () => queryDispatcher.Dispatch<QueryStub, string>(new QueryStub()));
        }

        [Test]
        public void TestDispatch_WhenQueryHandlerResolved_ThenQueryIsHandled()
        {
            Mock<IQueryHandler<QueryStub, string>> queryHandlerMock = new();
            queryHandlerMock
                .Setup(q => q.Handle(It.IsAny<QueryStub>()))
                .Returns("QueryResult");
            Mock<IDependencyResolver> dependencyResolverMock = new();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(queryHandlerMock.Object);
            QueryDispatcher queryDispatcher = new(dependencyResolverMock.Object);

            string result = queryDispatcher.Dispatch<QueryStub, string>(new QueryStub());

            Assert.That(result, Is.EqualTo("QueryResult"));
        }

        [Test]
        public void TestDispatchAsync_WhenNullQueryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(() => null!);
            QueryDispatcher queryDispatcher = new(dependencyResolverMock.Object);

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
            Mock<IDependencyResolver> dependencyResolverMock = new();
            dependencyResolverMock
                .Setup(d => d.Resolve<IQueryHandler<QueryStub, string>>())
                .Returns(() => null!);
            QueryDispatcher queryDispatcher = new(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<QueryHandlerNotFoundException>(),
                () => queryDispatcher.DispatchAsync<QueryStub, string>(new QueryStub()).Wait());
        }

        [Test]
        public async Task TestDispatchAsync_WhenQueryHandlerResolved_ThenQueryIsHandled()
        {
            Mock<IAsyncQueryHandler<QueryStub, string>> queryHandlerMock = new();
            queryHandlerMock.Setup(q => q.HandleAsync(It.IsAny<QueryStub>()))
                .ReturnsAsync("QueryResult");
            Mock<IDependencyResolver> dependencyResolverMock = new();
            dependencyResolverMock
                .Setup(d => d.Resolve<IAsyncQueryHandler<QueryStub, string>>())
                .Returns(queryHandlerMock.Object);
            QueryDispatcher queryDispatcher = new(dependencyResolverMock.Object);

            string result = await queryDispatcher.DispatchAsync<QueryStub, string>(new QueryStub());

            Assert.That(result, Is.EqualTo("QueryResult"));
        }
    }
}