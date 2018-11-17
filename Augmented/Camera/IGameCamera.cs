namespace Augmented
{
    public interface IGameCamera : ICamera
    {
        CameraMovement GameUpdateContinuousMovement { get; set; }
        void Move(CameraMovement cameraMovement, float magnitude);
        void Zoom(int magnitude);
    }
}