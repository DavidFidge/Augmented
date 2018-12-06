using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DavidFidge.MonoGame.Core.Graphics.Camera
{
    public abstract class BaseCamera : ICamera
    {
        private readonly IGameProvider _gameProvider;
        private int _projectionAngle = 90;
        private float _nearClippingPlane = 0.5f;
        private float _farClippingPlane = 10000f;

        protected float _viewportWidth;
        protected float _viewportHeight;
        protected int _fieldOfView;
        protected Vector3 _cameraPosition;
        protected Vector3 _cameraLookAt;
        protected Matrix _viewRotation;

        public Matrix View { get; protected set; }
        public Matrix Projection { get; private set; }

        protected BaseCamera(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
            _cameraPosition = new Vector3(0f, 0f, 100f);
            _viewRotation = Matrix.Identity;
            _cameraLookAt = new Vector3(0.0f, 0.0f, 0.0f);
            SetViewMatrix();
        }

        public abstract void Update();
        public abstract void Reset(float z, CameraResetOptions cameraResetOptions);

        public void Initialise()
        {
            Reset(100f, CameraResetOptions.AbsoluteZ);
            RecalculateProjectionMatrix();
        }

        public void RecalculateProjectionMatrix()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(_projectionAngle),
                _gameProvider.Game.GraphicsDevice.Viewport.AspectRatio,
                _nearClippingPlane,
                _farClippingPlane
            );

            _viewportHeight = _gameProvider.Game.GraphicsDevice.Viewport.Height;
            _viewportWidth = _gameProvider.Game.GraphicsDevice.Viewport.Width;
        }

        public Ray GetPointerRay(MouseState m)
        {
            var nearScreenPoint = new Vector3(m.X, m.Y, 0);
            var farScreenPoint = new Vector3(m.X, m.Y, 1);

            var near3DPoint = _gameProvider.Game.GraphicsDevice.Viewport.Unproject(nearScreenPoint, Projection, View, Matrix.Identity);
            var far3DPoint = _gameProvider.Game.GraphicsDevice.Viewport.Unproject(farScreenPoint, Projection, View, Matrix.Identity);

            var pointerRayDirection = far3DPoint - near3DPoint;

            pointerRayDirection.Normalize();

            return new Ray(near3DPoint, pointerRayDirection);
        }

        protected void SetViewMatrix()
        {
            var lookAtMatrix = Matrix.CreateLookAt(
                _cameraPosition,
                _cameraLookAt,
                Vector3.Up
            );

            View = _viewRotation * lookAtMatrix;
        }

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
            _cameraLookAt.X += x;
            _cameraLookAt.Y += y;
            _cameraLookAt.Z += z;
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

        public Matrix TranslationMatrix => Matrix.CreateTranslation(_cameraPosition);

        public Matrix ScaleMatrix => Matrix.Identity;
    }
}