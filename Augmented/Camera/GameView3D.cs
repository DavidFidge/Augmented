using DavidFidge.MonoGame.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Augmented
{
    public class GameView3D
    {
        private readonly IGameProvider _gameProvider;
        public IGameCamera Camera { get; }

        private int _projectionAngle = 90;

        public Matrix ProjectionMatrix { get; private set; }
        
        public GameView3D(IGameCamera gameCamera, IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
            Camera = gameCamera;
        }

        public void RecalculateProjectionMatrix()
        {
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(_projectionAngle),
                (float)_gameProvider.Game.GraphicsDevice.Viewport.Width /
                (float)_gameProvider.Game.GraphicsDevice.Viewport.Height,
                0.5f, 1000.0f
            );

            Camera.SetViewport((float)_gameProvider.Game.GraphicsDevice.Viewport.Width, (float)_gameProvider.Game.GraphicsDevice.Viewport.Height, _projectionAngle);
        }

        public Ray GetpointerRay(MouseState m)
        {
            var nearScreenPoint = new Vector3(m.X, m.Y, 0);
            var farScreenPoint = new Vector3(m.X, m.Y, 1);

            var near3DPoint = _gameProvider.Game.GraphicsDevice.Viewport.Unproject(nearScreenPoint, ProjectionMatrix, Camera.ViewTransform, Matrix.Identity);
            var far3DPoint = _gameProvider.Game.GraphicsDevice.Viewport.Unproject(farScreenPoint, ProjectionMatrix, Camera.ViewTransform, Matrix.Identity);

            var pointerRayDirection = far3DPoint - near3DPoint;

            pointerRayDirection.Normalize();

            return new Ray(near3DPoint, pointerRayDirection);
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update();
        }
    }
}