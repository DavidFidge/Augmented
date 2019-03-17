using System;

using DavidFidge.MonoGame.Core.Graphics.Camera;
using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics.Camera
{
    public class FreeGameCamera : BaseCamera, IGameCamera
    {
        protected float _moveSpeed = 1f;
        protected float _zoomSpeed = 0.1f;
        private Quaternion _cameraRotation;

        public CameraMovement GameUpdateContinuousMovement { get; set; }

        public FreeGameCamera(IGameProvider gameProvider) : base(gameProvider)
        {
            Reset();
        }

        public override void Reset()
        {
            _cameraPosition = new Vector3(0f, 0f, 100f);
            _cameraRotation = Quaternion.Identity;
            SetViewMatrix();
        }

        public override void Update()
        {
            Move(GameUpdateContinuousMovement, _moveSpeed);
            SetViewMatrix();
        }

        public void Move(CameraMovement cameraMovement, float magnitude)
        {
            var movementVector = new Vector3();
            var upDownRotation = 0f;
            var leftRightRotation = 0f;

            if (cameraMovement.HasFlag(CameraMovement.PanLeft))
                movementVector.X -= magnitude;

            if (cameraMovement.HasFlag(CameraMovement.PanRight))
                movementVector.X += magnitude;

            if (cameraMovement.HasFlag(CameraMovement.PanUp))
                movementVector.Y += magnitude;

            if (cameraMovement.HasFlag(CameraMovement.PanDown))
                movementVector.Y -= magnitude;

            if (cameraMovement.HasFlag(CameraMovement.Forward))
                movementVector.Z -= magnitude;

            if (cameraMovement.HasFlag(CameraMovement.Backward))
                movementVector.Z += magnitude;

            if (cameraMovement.HasFlag(CameraMovement.RotateUp))
                upDownRotation -= magnitude;

            if (cameraMovement.HasFlag(CameraMovement.RotateDown))
                upDownRotation += magnitude;

            if (cameraMovement.HasFlag(CameraMovement.RotateLeft))
                leftRightRotation += magnitude;

            if (cameraMovement.HasFlag(CameraMovement.RotateRight))
                leftRightRotation -= magnitude;


            var additionalRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, upDownRotation) * Quaternion.CreateFromAxisAngle(Vector3.Right, leftRightRotation);

            _cameraRotation = _cameraRotation * additionalRotation;

            var rotatedVector = Vector3.Transform(movementVector, _cameraRotation);

            ChangeTranslationRelative(rotatedVector);
        }

        protected override void SetViewMatrix()
        {
            var cameraRotatedTarget = Vector3.Transform(Vector3.Forward, _cameraRotation);
            var cameraFinalTarget = _cameraPosition + cameraRotatedTarget;
            var cameraRotatedUpVector = Vector3.Transform(Vector3.Up, _cameraRotation);

            View = Matrix.CreateLookAt(_cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

        public void Zoom(int magnitude)
        {
            if (magnitude == 0)
                return;

            Move(
                magnitude > 0 ? CameraMovement.Forward : CameraMovement.Backward,
                Math.Abs(magnitude) * _zoomSpeed
            );
        }
    }
}