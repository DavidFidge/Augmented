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

        public Matrix View { get; protected set; }
        public Matrix Projection { get; private set; }

        protected BaseCamera(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
        }

        public abstract void Update();
        public abstract void Reset();

        public void Initialise()
        {
            Reset();
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

        protected abstract void SetViewMatrix();

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

        public virtual void ChangeRotationRelative(float x, float y, float z)
        {
        }

        public virtual void ChangeRotation(float x, float y, float z)
        {
        }

        public Vector3 Translation => _cameraPosition;

        public Vector3 Scale => new Vector3(1f);

        public virtual Matrix Rotation => Matrix.Identity;

        public Matrix TranslationMatrix => Matrix.CreateTranslation(_cameraPosition);

        public Matrix ScaleMatrix => Matrix.Identity;
    }
}