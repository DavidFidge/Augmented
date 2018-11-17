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
            _gameView3D.Initialise();
        }

        public override void Update()
        {
            _gameView3D.Update();
        }

        public override void Draw()
        {
            _gameView3D.Draw();
            base.Draw();
        }
    }
}