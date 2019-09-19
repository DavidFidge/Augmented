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
            if (ActionMap.ActionIs<CloseConsoleRequest>(keyInFocus, keyboardModifier))
                Mediator.Send(new CloseConsoleRequest());

            if (ActionMap.ActionIs<SendConsoleCommandRequest>(keyInFocus, keyboardModifier))
                Mediator.Send(new SendConsoleCommandRequest());
        }
    }
}