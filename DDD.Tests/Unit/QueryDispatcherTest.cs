using DDD.CQRS;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DDD.Tests.Unit
{
    [TestFixture]
    public class QueryDispatcherTest
    {
        public class TestQuery : IQuery<object>
        {
        }

        [Test]
        public void TestCreating()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("dependencyResolver"),
                () => new QueryDispatcher(null));
        }

        [Test]
        public void TestDispatchValidation()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<IQueryHandler<TestQuery, object>>()).Returns(() => null);
            QueryDispatcher queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("query"),
                () => queryDispatcher.Dispatch<TestQuery, object>(null));
            Assert.Throws(
                Is.InstanceOf<QueryHandlerNotFoundException>(),
                () => queryDispatcher.Dispatch<TestQuery, object>(new TestQuery()));
        }

        [TestCase("1")]
        [TestCase(0)]
        [TestCase(201212.2)]
        public void TestDispatch(object @object)
        {
            Mock<IQueryHandler<TestQuery, object>> queryHandlerMock = new Mock<IQueryHandler<TestQuery, object>>();
            queryHandlerMock.Setup(q => q.Handle(It.IsAny<TestQuery>())).Returns(@object);
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<IQueryHandler<TestQuery, object>>()).Returns(queryHandlerMock.Object);
            QueryDispatcher queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            object result = queryDispatcher.Dispatch<TestQuery, object>(new TestQuery());

            Assert.That(result, Is.EqualTo(@object));
        }

        [Test]
        public void TestDispatchAsyncValidation()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<IQueryHandler<TestQuery, object>>()).Returns(() => null);
            QueryDispatcher queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("query"),
                () => queryDispatcher.DispatchAsync<TestQuery, object>(null).Wait());
            Assert.Throws(
                Is.InstanceOf<QueryHandlerNotFoundException>(),
                () => queryDispatcher.DispatchAsync<TestQuery, object>(new TestQuery()).Wait());
        }

        [TestCase("1")]
        [TestCase(0)]
        [TestCase(201212.2)]
        public async Task TestDispatchAsync(object @object)
        {
            Mock<IAsyncQueryHandler<TestQuery, object>> queryHandlerMock = new Mock<IAsyncQueryHandler<TestQuery, object>>();
            queryHandlerMock.Setup(q => q.HandleAsync(It.IsAny<TestQuery>())).ReturnsAsync(@object);
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<IAsyncQueryHandler<TestQuery, object>>()).Returns(queryHandlerMock.Object);
            QueryDispatcher queryDispatcher = new QueryDispatcher(dependencyResolverMock.Object);

            var result = await queryDispatcher.DispatchAsync<TestQuery, object>(new TestQuery());

            Assert.That(result, Is.EqualTo(@object));
        }
    }
}