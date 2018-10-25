using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.Services;
using DavidFidge.TestInfrastructure;

using InputHandlers.Keyboard;
using InputHandlers.Mouse;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework.Input;

using NSubstitute;

namespace DavidFidge.MonoGame.Core.Tests.Services
{
    [TestClass]
    public class GameInputServiceTests : BaseTest
    {
        private IMouseInput _mouseInput;
        private IKeyboardInput _keyboardInput;
        private GameInputService _gameInputService;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _mouseInput = Substitute.For<IMouseInput>();
            _keyboardInput = Substitute.For<IKeyboardInput>();

            _gameInputService = new GameInputService(_mouseInput, _keyboardInput);
        }

        [TestMethod]
        public void ChangeInput_Should_Subscribe_Mouse_And_Keyboard_Handlers()
        {
            // Arrange
            var mouseHandler = Substitute.For<IMouseHandler>();
            var keyboardHandler = Substitute.For<IKeyboardHandler>();

            // Act
            _gameInputService.ChangeInput(keyboardHandler, mouseHandler);
            
            // Assert
            _mouseInput.Received().Subscribe(Arg.Is(mouseHandler));
            _keyboardInput.Received().Subscribe(Arg.Is(keyboardHandler));
        }

        [TestMethod]
        public void ChangeInput_Should_Unsubscribe_Mouse_And_Keyboard_Handlers()
        {
            // Arrange
            var mouseHandlerSubscribed = Substitute.For<IMouseHandler>();
            var keyboardHandlerSubscribed = Substitute.For<IKeyboardHandler>();

            var mouseHandlerNew = Substitute.For<IMouseHandler>();
            var keyboardHandlerNew = Substitute.For<IKeyboardHandler>();

            _gameInputService.ChangeInput(keyboardHandlerSubscribed, mouseHandlerSubscribed);

            _mouseInput.ClearReceivedCalls();
            _keyboardInput.ClearReceivedCalls();

            // Act
            _gameInputService.ChangeInput(keyboardHandlerNew, mouseHandlerNew);

            // Assert
            _mouseInput.Received().Subscribe(Arg.Is(mouseHandlerNew));
            _keyboardInput.Received().Subscribe(Arg.Is(keyboardHandlerNew));

            _mouseInput.Received().Unsubscribe(Arg.Is(mouseHandlerSubscribed));
            _keyboardInput.Received().Unsubscribe(Arg.Is(keyboardHandlerSubscribed));
        }
        
        [TestMethod]
        public void RevertInput_Should_Subscribe_Previous_Mouse_And_Keyboard_Handlers()
        {
            // Arrange
            var mouseHandlerToRevertTo = Substitute.For<IMouseHandler>();
            var keyboardHandlerToRevertTo = Substitute.For<IKeyboardHandler>();

            var mouseHandlerNew = Substitute.For<IMouseHandler>();
            var keyboardHandlerNew = Substitute.For<IKeyboardHandler>();

            _gameInputService.ChangeInput(keyboardHandlerToRevertTo, mouseHandlerToRevertTo);
            _gameInputService.ChangeInput(keyboardHandlerNew, mouseHandlerNew);

            _mouseInput.ClearReceivedCalls();
            _keyboardInput.ClearReceivedCalls();

            // Act
            _gameInputService.RevertInput();

            // Assert
            _mouseInput.Received().Subscribe(Arg.Is(mouseHandlerToRevertTo));
            _keyboardInput.Received().Subscribe(Arg.Is(keyboardHandlerToRevertTo));

            _mouseInput.Received().Unsubscribe(Arg.Is(mouseHandlerNew));
            _keyboardInput.Received().Unsubscribe(Arg.Is(keyboardHandlerNew));
        }

        [TestMethod]
        public void RevertInput_Should_Do_Nothing_If_No_Subscriptions()
        {
            // Act
            _gameInputService.RevertInput();

            // Assert
            Assert.AreEqual(0, _mouseInput.ReceivedCalls().Count());
            Assert.AreEqual(0, _keyboardInput.ReceivedCalls().Count());
        }
        
        [TestMethod]
        public void Poll_Should_Poll_Both_Keyboard_And_Mouse_Handlers()
        {
            // Act
            _gameInputService.Poll();

            // Assert
            _mouseInput.Received().Poll(Arg.Any<MouseState>());
            _keyboardInput.Received().Poll(Arg.Any<KeyboardState>());
        }
        
        [TestMethod]
        public void Reset_Should_Reset_Both_Keyboard_And_Mouse_Handlers()
        {
            // Act
            _gameInputService.Reset();

            // Assert
            _mouseInput.Received().Reset();
            _keyboardInput.Received().Reset();
        }
    }
}