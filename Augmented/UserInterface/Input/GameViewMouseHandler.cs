using Augmented.Messages;

using DavidFidge.MonoGame.Core.UserInterface;

using Microsoft.Xna.Framework.Input;

namespace Augmented.UserInterface.Input
{
    public class GameViewMouseHandler : BaseMouseHandler
    {
        public override void HandleMouseScrollWheelMove(MouseState mouseState, int difference)
        {
            Mediator.Send(new Zoom3DViewRequest(difference));
        }
    }
}