using DavidFidge.MonoGame.Core.Graphics.Camera;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

namespace Augmented.Graphics.Camera
{
    public interface IGameCamera : ICamera
    {
        CameraMovement GameUpdateContinuousMovement { get; set; }
        void Move(CameraMovement cameraMovement, float magnitude);
        void Zoom(int magnitude);
    }
}