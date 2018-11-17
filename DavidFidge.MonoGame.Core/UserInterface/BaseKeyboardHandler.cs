using DavidFidge.MonoGame.Core.Components;

using InputHandlers.Keyboard;

using Microsoft.Xna.Framework.Input;

namespace DavidFidge.MonoGame.Core.UserInterface
{
    public abstract class BaseKeyboardHandler : BaseComponent, IKeyboardHandler
    {
        public virtual void HandleKeyboardKeyDown(Keys[] keysDown, Keys keyInFocus, KeyboardModifier keyboardModifier)
        {
        }

        public virtual void HandleKeyboardKeyLost(Keys[] keysDown, KeyboardModifier keyboardModifier)
        {
        }

        public virtual void HandleKeyboardKeyRepeat(Keys repeatingKey, KeyboardModifier keyboardModifier)
        {
        }

        public virtual void HandleKeyboardKeysReleased()
        {
        }
    }
}