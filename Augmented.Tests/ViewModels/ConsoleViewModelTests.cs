using System.Linq;
using System.Threading;

using Augmented.Messages;
using Augmented.UserInterface.Data;
using Augmented.UserInterface.ViewModels;

using DavidFidge.MonoGame.Core.ConsoleCommands;
using DavidFidge.MonoGame.Core.Interfaces.ConsoleCommands;
using DavidFidge.MonoGame.Core.Messages;
using DavidFidge.TestInfrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace Augmented.Tests.ViewModels
{
    [TestClass]
    public class ConsoleViewModelTests : BaseTest
    {
        private ConsoleViewModel _consoleViewModel;
        private IConsoleCommandServiceFactory _consoleCommandServiceFactory;
        private IConsoleCommand _consoleCommand;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _consoleCommandServiceFactory = Substitute.For<IConsoleCommandServiceFactory>();

            _consoleCommand = Substitute.For<IConsoleCommand>();

            _consoleViewModel = SetupBaseComponent(new ConsoleViewModel(_consoleCommandServiceFactory));

            _consoleViewModel.Initialize();
        }

        [TestMethod]
        public void Should_Add_To_LastCommands_When_Command_Executes()
        {
            // Arrange
            _consoleCommandServiceFactory
                .CommandFor(Arg.Is<ConsoleCommand>(c => c.Name == "Test"))
                .Returns(_consoleCommand);

            _consoleCommand
                .When(c => c.Execute(Arg.Any<ConsoleCommand>()))
                .Do(ci => ci.Arg<ConsoleCommand>().Result = "ExecuteCalled");

            // Act
            _consoleViewModel.Handle(new ExecuteConsoleCommandRequest("Test"), new CancellationToken());

            // Assert
            Assert.AreEqual(1, _consoleViewModel.Data.LastCommands.Count);

            var command = _consoleViewModel.Data.LastCommands.First;
            Assert.AreEqual("Test", command.Value.Name);
            Assert.AreEqual("ExecuteCalled", command.Value.Result);
        }

        [TestMethod]
        public void Should_Return_CommandNotFound_When_Command_Not_Returned_From_Factory()
        {
            // Arrange
            _consoleCommandServiceFactory
                .CommandFor(Arg.Is<ConsoleCommand>(c => c.Name == "Test"))
                .Returns((IConsoleCommand)null);

            // Act
            _consoleViewModel.Handle(new ExecuteConsoleCommandRequest("Test"), new CancellationToken());

            // Assert
            Assert.AreEqual(1, _consoleViewModel.Data.LastCommands.Count);

            var command = _consoleViewModel.Data.LastCommands.First;
            Assert.AreEqual("Test", command.Value.Name);
            Assert.AreEqual("Command not found", command.Value.Result);
        }

        [TestMethod]
        public void Should_Notify_When_Command_Is_Handled()
        {
            // Arrange
            _consoleCommandServiceFactory
                .CommandFor(Arg.Is<ConsoleCommand>(c => c.Name == "Test"))
                .Returns(_consoleCommand);

            // Act
            _consoleViewModel.Handle(new ExecuteConsoleCommandRequest("Test"), new CancellationToken());

            // Assert
            _consoleViewModel.Mediator
                .Received()
                .Send(Arg.Any<UpdateViewRequest<ConsoleData>>());
        }

        [TestMethod]
        public void Should_Notify_When_Command_Not_Found()
        {
            // Arrange
            _consoleCommandServiceFactory
                .CommandFor(Arg.Is<ConsoleCommand>(c => c.Name == "Test"))
                .Returns((IConsoleCommand)null);

            // Act
            _consoleViewModel.Handle(new ExecuteConsoleCommandRequest("Test"), new CancellationToken());

            // Assert
            _consoleViewModel.Mediator
                .Received()
                .Send(Arg.Any<UpdateViewRequest<ConsoleData>>());
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void Should_Do_Nothing_For_Null_Or_Empty_String(string command)
        {
            // Act
            _consoleViewModel.Handle(new ExecuteConsoleCommandRequest(command), new CancellationToken());

            // Assert
            _consoleViewModel.Mediator
                .DidNotReceive()
                .Send(Arg.Any<UpdateViewRequest<ConsoleData>>());

            Assert.IsTrue(_consoleViewModel.Data.LastCommands.IsEmpty());
        }
    }
}