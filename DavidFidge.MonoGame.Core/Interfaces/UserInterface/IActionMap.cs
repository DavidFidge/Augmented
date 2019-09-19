using DavidFidge.MonoGame.Core.UserInterface;

using InputHandlers.Keyboard;

using Microsoft.Xna.Framework.Input;

namespace DavidFidge.MonoGame.Core.Interfaces.UserInterface
{
    public interface IActionMap
    {
        bool ActionIs<T>(Keys key, KeyboardModifier keyboardModifier);
        bool ActionIs<T>(KeyCombination keyCombination);
    }
}