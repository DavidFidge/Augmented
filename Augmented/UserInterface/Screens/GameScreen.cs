using Augmented.Graphics.Camera;
using Augmented.Messages;
using Augmented.UserInterface.Views;

using DavidFidge.MonoGame.Core.Interfaces.Services;
using DavidFidge.Monogame.Core.View;

namespace Augmented.UserInterface.Screens
{
    public class GameScreen : Screen
    {
        private readonly GameView3D _gameView3D;
        private readonly IGameTimeService _gameTimeService;

        public GameScreen(
            GameView gameView,
            GameView3D gameView3D,
            IGameTimeService gameTimeService
            ) : base(gameView)
        {
            _gameView3D = gameView3D;
            _gameTimeService = gameTimeService;
        }

        public void EndGame()
        {
            _gameTimeService.Stop();
        }

        public void StartNewGame()
        {
            Mediator.Send(new ChangeGameSpeedRequest().ResetRequest());

            _gameView3D.Initialise();
            _gameTimeService.Start();
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