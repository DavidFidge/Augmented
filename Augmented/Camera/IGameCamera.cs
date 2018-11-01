using Microsoft.Xna.Framework;

namespace Augmented
{
    public interface IGameCamera : ITransformable
    {
        void Reset(float z, CameraResetOptions cameraResetOptions);
        void KeyboardPan(CameraMovement cameraMovement, uint updateNumber);
        void KeyboardEndPan();
        void MousePan();
        void MouseZoom(int z);
        void SetViewport(float width, float height, int fieldOfView);
        Matrix ViewTransform { get; }
        void Update();
    }
}