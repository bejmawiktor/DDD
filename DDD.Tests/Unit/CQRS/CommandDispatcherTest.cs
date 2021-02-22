using DDD.CQRS;
using DDD.Tests.Unit.TestDoubles;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.CQRS
{
    [TestFixture]
    public class CommandDispatcherTest
    {
        [Test]
        public void TestConstructing_WhenNullDependencyResolverGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("dependencyResolver"),
                () => new CommandDispatcher(null));
        }

        [Test]
        public void TestDispatch_WhenNullCommandGiven_ThenArgumentNullExceptionIsThrown()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<ICommandHandler<CommandStub>>()).Returns(() => null);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("command"),
                () => commandDispatcher.Dispatch<CommandStub>(null));
            Assert.Throws(
                Is.InstanceOf<CommandHandlerNotFoundException>(),
                () => commandDispatcher.Dispatch(new CommandStub()));
        }

        [Test]
        public void TestDispatch_WhenNotResolvableCommandGiven_ThenCommandHandlerNotFoundExceptionIsThrown()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<ICommandHandler<CommandStub>>()).Returns(() => null);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<CommandHandlerNotFoundException>(),
                () => commandDispatcher.Dispatch(new CommandStub()));
        }

        [Test]
        public void TestDispatch_WhenCommandHandlerResolved_ThenCommandIsHandled()
        {
            Mock<ICommandHandler<CommandStub>> commandHandlerMock = new Mock<ICommandHandler<CommandStub>>();
            commandHandlerMock.Setup(q => q.Handle(It.IsAny<CommandStub>()));
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<ICommandHandler<CommandStub>>()).Returns(commandHandlerMock.Object);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            commandDispatcher.Dispatch(new CommandStub());

            commandHandlerMock.Verify(c => c.Handle(It.IsAny<CommandStub>()), Times.Once);
        }

        [Test]
        public void TestDispatchAsync_WhenNullCommandGiven_ThenArgumentNullExceptionIsThrown()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<IAsyncCommandHandler<CommandStub>>()).Returns(() => null);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("command"),
                () => commandDispatcher.DispatchAsync<CommandStub>(null).Wait());
        }

        [Test]
        public void TestDispatchAsync_WhenNotResolvableCommandGiven_ThenCommandHandlerNotFoundExceptionIsThrown()
        {
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<IAsyncCommandHandler<CommandStub>>()).Returns(() => null);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            Assert.Throws(
                Is.InstanceOf<CommandHandlerNotFoundException>(),
                () => commandDispatcher.DispatchAsync(new CommandStub()).Wait());
        }

        [Test]
        public async Task TestDispatchAsync_WhenCommandHandlerResolved_ThenCommandIsHandled()
        {
            Mock<IAsyncCommandHandler<CommandStub>> commandHandlerMock = new Mock<IAsyncCommandHandler<CommandStub>>();
            commandHandlerMock.Setup(q => q.HandleAsync(It.IsAny<CommandStub>()));
            Mock<IDependencyResolver> dependencyResolverMock = new Mock<IDependencyResolver>();
            dependencyResolverMock.Setup(d => d.Resolve<IAsyncCommandHandler<CommandStub>>()).Returns(commandHandlerMock.Object);
            CommandDispatcher commandDispatcher = new CommandDispatcher(dependencyResolverMock.Object);

            await commandDispatcher.DispatchAsync(new CommandStub());

            commandHandlerMock.Verify(c => c.HandleAsync(It.IsAny<CommandStub>()), Times.Once);
        }
    }
}