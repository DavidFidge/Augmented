using DavidFidge.MonoGame.Core.Graphics.Camera;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Interfaces.Graphics
{
    public interface ICamera : ITransform
    {
        void Reset(float z, CameraResetOptions cameraResetOptions);
        Matrix View { get; }
        Matrix Projection { get; }
        void Update();
        void Initialise();
    }
}