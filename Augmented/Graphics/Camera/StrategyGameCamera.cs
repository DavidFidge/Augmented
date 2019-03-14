using System;

using DavidFidge.MonoGame.Core.Graphics.Camera;
using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics.Camera
{
    public class StrategyGameCamera : BaseCamera, IGameCamera
    {
        protected float _zoomMin = float.MinValue;
        protected float _zoomMax = float.MaxValue;
        protected float _moveSpeed = 1f;
        protected float _zoomSpeed = 0.1f;
        private float _rotateX;
        private float _rotateZ;

        public CameraMovement GameUpdateContinuousMovement { get; set; }

        public StrategyGameCamera(IGameProvider gameProvider) : base(gameProvider)
        {
            _viewRotation = Matrix.Identity;

            _cameraLookAt = new Vector3(0.0f, 0.0f, 0.0f);
            SetViewMatrix();
        }

        public override void Reset(float z, CameraResetOptions cameraResetOptions)
        {
            var percent = (_viewportHeight / _viewportWidth);

            switch (cameraResetOptions)
            {
                // internally perspective is applied on width, so need to adjust for width perspectiving here
                case CameraResetOptions.AbsoluteZ:
                    _cameraPosition = new Vector3(0.0f, 0.0f, z);
                    break;
                case CameraResetOptions.WidthOfObjectAtZero:
                    _cameraPosition = new Vector3(0.0f, 0.0f, z * percent / (float)(Math.Tan(MathHelper.ToRadians(_fieldOfView / 2.0f))));
                    break;
                case CameraResetOptions.HeightOfObjectAtZero:
                    _cameraPosition = new Vector3(0.0f, 0.0f, z / (float)(Math.Tan(MathHelper.ToRadians(_fieldOfView / 2.0f))));
                    break;
            }

            _cameraLookAt = new Vector3(0.0f, 0.0f, 0.0f);
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
                _rotateX -= magnitude;

            if (cameraMovement.HasFlag(CameraMovement.RotateDown))
                _rotateX += magnitude;

            if (cameraMovement.HasFlag(CameraMovement.RotateLeft))
                _rotateZ += magnitude;

            if (cameraMovement.HasFlag(CameraMovement.RotateRight))
                _rotateZ -= magnitude;

            // pan camera by adding scroll vectors
            ChangeTranslationRelative(movementVector);

            if (_cameraPosition.Z < _zoomMin)
                _cameraPosition.Z = _zoomMin;

            if (_cameraPosition.Z > _zoomMax)
                _cameraPosition.Z = _zoomMax;

            // set look at position, this changes to whatever the camera x and y is (not doing this will make camera rotate)
            _cameraLookAt.X = _cameraPosition.X;
            _cameraLookAt.Y = _cameraPosition.Y;

            _viewRotation = Matrix.CreateRotationX(_rotateX) * Matrix.CreateRotationY(_rotateZ) * Matrix.Identity;
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