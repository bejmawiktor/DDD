using DDD.CQRS;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DDD.Tests.Unit
{
    [TestFixture]
    public class CommandDispatcherTest
    {
        public class TestCommand : ICommand
        {
        }

        [Test]
        public void TestCreating()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("dependencyResolver"),
                () => new CommandDispatcher(null));
        }

        [Test]
        public void TestDispatchValidation()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<ICommandHandler<TestCommand>>()).Returns(() => null);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("command"),
                () => commandDispatcher.Dispatch<TestCommand>(null));
            Assert.Throws(
                Is.InstanceOf<CommandHandlerNotFoundException>(),
                () => commandDispatcher.Dispatch(new TestCommand()));
        }

        [Test]
        public void TestDispatch()
        {
            Mock<ICommandHandler<TestCommand>> commandHandlerMock = new Mock<ICommandHandler<TestCommand>>();
            commandHandlerMock.Setup(q => q.Handle(It.IsAny<TestCommand>()));
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<ICommandHandler<TestCommand>>()).Returns(commandHandlerMock.Object);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            commandDispatcher.Dispatch(new TestCommand());

            commandHandlerMock.Verify(c => c.Handle(It.IsAny<TestCommand>()), Times.Once);
        }

        [Test]
        public void TestDispatchAsyncValidation()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<IAsyncCommandHandler<TestCommand>>()).Returns(() => null);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("command"),
                () => commandDispatcher.DispatchAsync<TestCommand>(null).Wait());
            Assert.Throws(
                Is.InstanceOf<CommandHandlerNotFoundException>(),
                () => commandDispatcher.DispatchAsync(new TestCommand()).Wait());
        }

        [Test]
        public async Task TestDispatchAsync()
        {
            Mock<IAsyncCommandHandler<TestCommand>> commandHandlerMock = new Mock<IAsyncCommandHandler<TestCommand>>();
            commandHandlerMock.Setup(q => q.HandleAsync(It.IsAny<TestCommand>()));
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<IAsyncCommandHandler<TestCommand>>()).Returns(commandHandlerMock.Object);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            await commandDispatcher.DispatchAsync(new TestCommand());

            commandHandlerMock.Verify(c => c.HandleAsync(It.IsAny<TestCommand>()), Times.Once);
        }
    }
}