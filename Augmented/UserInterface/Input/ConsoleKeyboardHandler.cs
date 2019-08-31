using Augmented.Messages;

using DavidFidge.MonoGame.Core.UserInterface;

using InputHandlers.Keyboard;

using Microsoft.Xna.Framework.Input;

namespace Augmented.UserInterface.Input
{
    public class ConsoleKeyboardHandler : BaseKeyboardHandler
    {
        public override void HandleKeyboardKeyDown(Keys[] keysDown, Keys keyInFocus, KeyboardModifier keyboardModifier)
        {
            if (keyInFocus == Keys.OemTilde)
                Mediator.Send(new CloseConsoleRequest());

            if (keyInFocus == Keys.Enter)
                Mediator.Send(new SendConsoleCommandRequest());
        }
    }
}