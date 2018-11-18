using InputHandlers.Keyboard;
using InputHandlers.Mouse;

namespace DavidFidge.MonoGame.Core.Interfaces.Services
{
    public interface IGameInputService
    {
        void Poll();
        void Reset();
        void ChangeInput(IKeyboardHandler keyboardHandler, IMouseHandler mouseHandler);
        void RevertInput();
    }
}