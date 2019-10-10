using Augmented.Graphics.Camera;
using Augmented.Interfaces;
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
        private readonly IAugmentedGameWorldFactory _augmentedGameWorldFactory;
        private IAugmentedGameWorld _augmentedGameWorld;

        public GameScreen(
            GameView gameView,
            GameView3D gameView3D,
            IGameTimeService gameTimeService,
            IAugmentedGameWorldFactory augmentedGameWorldFactory
            ) : base(gameView)
        {
            _gameView3D = gameView3D;
            _gameTimeService = gameTimeService;
            _augmentedGameWorldFactory = augmentedGameWorldFactory;
        }

        public void EndGame()
        {
            _gameTimeService.Stop();
            _augmentedGameWorldFactory.Release(_augmentedGameWorld);
        }

        public void StartNewGame()
        {
            _augmentedGameWorld = _augmentedGameWorldFactory.Create();
            _augmentedGameWorld.SceneGraph.LoadContent();

            Mediator.Send(new ChangeGameSpeedRequest().ResetRequest());

            _gameView3D.Initialise();
            _gameTimeService.Start();
        }

        public override void Update()
        {
            _augmentedGameWorld.Update();
            _gameView3D.Update();
        }

        public override void Draw()
        {
            _gameView3D.Draw(_augmentedGameWorld.SceneGraph);
            base.Draw();
        }
    }
}