using Augmented.Messages;

using DavidFidge.MonoGame.Core.UserInterface;

using InputHandlers.Keyboard;

using Microsoft.Xna.Framework.Input;

namespace Augmented.UserInterface.Input
{
    public class GameSpeedKeyboardHandler : BaseKeyboardHandler
    {
        public override void HandleKeyboardKeyDown(Keys[] keysDown, Keys keyInFocus, KeyboardModifier keyboardModifier)
        {
            switch (keyInFocus)
            {
                case Keys.Subtract:
                case Keys.OemMinus:
                    Mediator.Send(new ChangeGameSpeedRequest().DecreaseSpeedRequest());
                    break;
                case Keys.Add:
                case Keys.OemPlus:
                    Mediator.Send(new ChangeGameSpeedRequest().IncreaseSpeedRequest());
                    break;
                case Keys.Space:
                    Mediator.Send(new ChangeGameSpeedRequest().TogglePauseGameRequest());
                    break;
            }
        }
    }
}