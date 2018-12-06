using System;
using System.Linq;
using Augmented.Messages;

using DavidFidge.MonoGame.Core.Graphics.Camera;
using DavidFidge.MonoGame.Core.UserInterface;

using InputHandlers.Keyboard;

using Microsoft.Xna.Framework.Input;

namespace Augmented.UserInterface.Input
{
    public class GameViewKeyboardHandler : BaseKeyboardHandler
    {
        public override void HandleKeyboardKeyDown(Keys[] keysDown, Keys keyInFocus, KeyboardModifier keyboardModifier)
        {
            if (keyInFocus == Keys.Escape)
                Mediator.Send(new OpenInGameOptionsRequest());

            if (keyInFocus == Keys.F12)
                Environment.Exit(0);

            SetCameraContinousMovement(keysDown);
        }

        public override void HandleKeyboardKeyLost(Keys[] keysDown, KeyboardModifier keyboardModifier)
        {
            SetCameraContinousMovement(keysDown);
        }

        public override void HandleKeyboardKeysReleased()
        {
            Mediator.Send(new Pan3DViewRequest(CameraMovement.None));
        }

        private void SetCameraContinousMovement(Keys[] keysDown)
        {
            var cameraMovementFlags = CameraMovement.None;

            if (keysDown.Contains(Keys.A) || keysDown.Contains(Keys.Left))
                cameraMovementFlags |= CameraMovement.PanLeft;

            if (keysDown.Contains(Keys.D) || keysDown.Contains(Keys.Right))
                cameraMovementFlags |= CameraMovement.PanRight;

            if (keysDown.Contains(Keys.W) || keysDown.Contains(Keys.Up))
                cameraMovementFlags |= CameraMovement.PanUp;

            if (keysDown.Contains(Keys.S) || keysDown.Contains(Keys.Down))
                cameraMovementFlags |= CameraMovement.PanDown;

            Mediator.Send(new Pan3DViewRequest(cameraMovementFlags));
        }
    }
}