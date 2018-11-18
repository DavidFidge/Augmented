using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.Interfaces.Services;

using InputHandlers.Keyboard;
using InputHandlers.Mouse;

using Microsoft.Xna.Framework.Input;

namespace DavidFidge.MonoGame.Core.Services
{
    public class GameInputService : IGameInputService
    {
        private readonly IMouseInput _mouseInput;
        private readonly IKeyboardInput _keyboardInput;

        private readonly Stack<IKeyboardHandler> _keyboardHandlers = new Stack<IKeyboardHandler>();
        private readonly Stack<IMouseHandler> _mouseHandlers = new Stack<IMouseHandler>();

        public GameInputService(IMouseInput mouseInput, IKeyboardInput keyboardInput)
        {
            _mouseInput = mouseInput;
            _keyboardInput = keyboardInput;
        }

        public void Poll()
        {
            _keyboardInput.Poll(Keyboard.GetState());
            _mouseInput.Poll(Mouse.GetState());
        }

        public void Reset()
        {
            _keyboardInput.Reset();
            _mouseInput.Reset();
        }

        public void ChangeInput(IKeyboardHandler keyboardHandler, IMouseHandler mouseHandler)
        {
            if (_keyboardHandlers.Any())
                _keyboardInput.Unsubscribe(_keyboardHandlers.Peek());

            if (_mouseHandlers.Any())
                _mouseInput.Unsubscribe(_mouseHandlers.Peek());

            _keyboardInput.Subscribe(keyboardHandler);
            _mouseInput.Subscribe(mouseHandler);

            _keyboardHandlers.Push(keyboardHandler);
            _mouseHandlers.Push(mouseHandler);
        }

        public void RevertInput()
        {
            if (_keyboardHandlers.Any())
                _keyboardInput.Unsubscribe(_keyboardHandlers.Pop());

            if (_mouseHandlers.Any())
                _mouseInput.Unsubscribe(_mouseHandlers.Pop());

            if (_keyboardHandlers.Any())
                _keyboardInput.Subscribe(_keyboardHandlers.Peek());

            if (_mouseHandlers.Any())
                _mouseInput.Subscribe(_mouseHandlers.Peek());
        }
    }
}