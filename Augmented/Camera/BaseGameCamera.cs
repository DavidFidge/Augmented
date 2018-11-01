using System;

using Microsoft.Xna.Framework;

namespace Augmented
{
    public abstract class BaseGameCamera : IGameCamera
    {
        protected float _viewportWidth;
        protected float _viewportHeight;
        protected int _fieldOfView;

        protected Vector3 _cameraPosition;
        protected Vector3 _cameraLookAt;

        public Matrix ViewTransform { get; protected set; }

        protected Matrix _viewRotation;

        protected BaseGameCamera()
        {
            _viewRotation = Matrix.Identity;
            _cameraLookAt = new Vector3(0.0f, 0.0f, 0.0f);
            SetViewMatrix();
        }

        public void SetViewport(float width, float height, int fieldOfView)
        {
            _viewportHeight = height;
            _viewportWidth = width;
            _fieldOfView = fieldOfView;
        }

        protected void SetViewMatrix()
        {
            var lookAtMatrix = Matrix.CreateLookAt(
                _cameraPosition,
                _cameraLookAt,
                Vector3.Up
            );

            ViewTransform = _viewRotation * lookAtMatrix;
        }

        public abstract void Update();
        public abstract void Reset(float z, CameraResetOptions cameraResetOptions);

        public abstract void KeyboardPan(CameraMovement cameraMovement, uint updateNumber);
        public abstract void KeyboardEndPan();
        public abstract void MousePan();
        public abstract void MouseZoom(int z);

        
        public void ChangeTranslationRelative(Vector3 translationDelta)
        {
            _cameraPosition += translationDelta;
        }

        public void ChangeTranslation(Vector3 translation)
        {
            _cameraPosition = translation;
        }

        public void ChangeScaleRelative(Vector3 scaleDelta)
        {
        }

        public void ChangeScale(Vector3 scale)
        {
        }

        public void ChangeRotationRelative(float x, float y, float z)
        {
            _cameraLookAt.X += x; _cameraLookAt.Y += y; _cameraLookAt.Z += z;
        }

        public void ChangeRotation(float x, float y, float z)
        {
            _cameraLookAt = new Vector3(x, y, z);
        }

        public Vector3 Translation => _cameraPosition;

        public Vector3 Scale => new Vector3(1f);

        public Matrix Rotation => Matrix.CreateLookAt(
            _cameraPosition,
            _cameraLookAt,
            Vector3.Up
        );

        public Matrix TranslationMatrix
        {
            get { throw new NotImplementedException(); }
        }

        public Matrix ScaleMatrix
        {
            get { throw new NotImplementedException(); }
        }
    }
}