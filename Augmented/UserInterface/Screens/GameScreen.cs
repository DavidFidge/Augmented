using Augmented.UserInterface.Views;

using DavidFidge.MonoGame.Core.UserInterface;

using Microsoft.Xna.Framework;

namespace Augmented.UserInterface.Screens
{
    public class GameScreen : Screen
    {
        private readonly GameView3D _gameView3D;

        public GameScreen(
            GameView gameView,
            GameView3D gameView3D
            ) : base(gameView)
        {
            _gameView3D = gameView3D;
        }

        public void EndGame()
        {
        }

        public void StartNewGame()
        {
        }

        public override void Update(GameTime gameTime)
        {
            _gameView3D.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _gameView3D.Dr
            base.Draw(gameTime);
        }
    }
}