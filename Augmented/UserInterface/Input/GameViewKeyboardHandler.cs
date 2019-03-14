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

            SetCameraContinuousMovement(keysDown);
        }

        public override void HandleKeyboardKeyLost(Keys[] keysDown, KeyboardModifier keyboardModifier)
        {
            SetCameraContinuousMovement(keysDown);
        }

        public override void HandleKeyboardKeysReleased()
        {
            Mediator.Send(new Pan3DViewRequest(CameraMovement.None));
        }

        private void SetCameraContinuousMovement(Keys[] keysDown)
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

            if (keysDown.Contains(Keys.Q))
                cameraMovementFlags |= CameraMovement.RotateLeft;

            if (keysDown.Contains(Keys.E))
                cameraMovementFlags |= CameraMovement.RotateRight;

            if (keysDown.Contains(Keys.PageUp) || keysDown.Contains(Keys.R))
                cameraMovementFlags |= CameraMovement.RotateUp;

            if (keysDown.Contains(Keys.PageDown) || keysDown.Contains(Keys.F))
                cameraMovementFlags |= CameraMovement.RotateDown;

            if (cameraMovementFlags != CameraMovement.None)
                Mediator.Send(new Pan3DViewRequest(cameraMovementFlags));

            if (keysDown.Contains(Keys.OemOpenBrackets) || keysDown.Contains(Keys.OemCloseBrackets))
            {
                var zoomMagnitude = 0;

                if (keysDown.Contains(Keys.OemOpenBrackets))
                    zoomMagnitude += 100;
                else if (keysDown.Contains(Keys.OemCloseBrackets))
                    zoomMagnitude -= 100;

                Mediator.Send(new Zoom3DViewRequest(zoomMagnitude));
            }
        }
    }
}