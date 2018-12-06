using DavidFidge.MonoGame.Core.Graphics.Camera;

using MediatR;

namespace Augmented.Messages
{
    public class Pan3DViewRequest : IRequest
    {
        public CameraMovement CameraMovementFlags { get; }

        public Pan3DViewRequest(CameraMovement cameraMovementFlags)
        {
            CameraMovementFlags = cameraMovementFlags;
        }
    }
}