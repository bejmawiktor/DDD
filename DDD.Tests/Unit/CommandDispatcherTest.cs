using DDD.CQRS;
using Moq;
using NUnit.Framework;
using System;

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
    }
}