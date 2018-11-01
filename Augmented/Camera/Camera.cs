using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Augmented
{
    public class GameCamera : BaseGameCamera
    {
        protected float _zoomMin = 1.0f;
        protected float _zoomMax = 500.0f;

        protected float _mouseZoomFactor;
        protected float _scrollSpeed;
        protected Vector2 _scrollVector;
        protected float _rotateValue;
        protected uint _lastUpdate;

        public GameCamera()
        {
            _mouseZoomFactor = 1.0f;
            _scrollSpeed = 0.5f;
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
            // pan camera by adding scroll vectors
            _cameraPosition.X -= _scrollVector.X;
            _cameraPosition.Y += _scrollVector.Y;

            // set look at position, this changes to whatever the camera x and y is (not doing this will make camera rotate)
            _cameraLookAt.X = _cameraPosition.X;
            _cameraLookAt.Y = _cameraPosition.Y;

            if (_rotateValue > 0 || _rotateValue < 0)
            {
                _viewRotation *= Matrix.CreateRotationX(_rotateValue / 10);
            }

            SetViewMatrix();
        }

        public override void KeyboardPan(CameraMovement cameraMovement, uint updateNumber)
        {
            if (updateNumber != _lastUpdate)
            {
                _scrollVector.X = 0.0f;
                _scrollVector.Y = 0.0f;
                _rotateValue = 0.0f;
                _lastUpdate = updateNumber;
            }

            switch (cameraMovement)
            {
                case CameraMovement.PanLeft:
                    _scrollVector.X += _scrollSpeed;
                    break;
                case CameraMovement.PanRight:
                    _scrollVector.X -= _scrollSpeed;
                    break;
                case CameraMovement.PanUp:
                    _scrollVector.Y += _scrollSpeed;
                    break;
                case CameraMovement.PanDown:
                    _scrollVector.Y -= _scrollSpeed;
                    break;
                case CameraMovement.RotateUp:
                    _rotateValue += _scrollSpeed;
                    break;
                case CameraMovement.RotateDown:
                    _rotateValue -= _scrollSpeed;
                    break;
            }
        }

        public override void KeyboardEndPan()
        {
            _scrollVector.X = 0.0f;
            _scrollVector.Y = 0.0f;
            _rotateValue = 0.0f;
        }

        public override void MousePan()
        {
            // if full screen mode, see if mouse at edge of screen, if so scroll
            throw new NotImplementedException();
        }

        public override void MouseZoom(int z)
        {
            _cameraPosition.Z -= z * 0.1f;

            if (_cameraPosition.Z < _zoomMin)
            {
                _cameraPosition.Z = _zoomMin;
            }
            if (_cameraPosition.Z > _zoomMax)
            {
                _cameraPosition.Z = _zoomMax;
            }
        }
    }
}