using System.Threading;
using System.Threading.Tasks;

using Augmented.Messages;
using DavidFidge.MonoGame.Core.UserInterface;
using MediatR;

using Microsoft.Xna.Framework;

namespace Augmented.UserInterface.Screens
{
    public class ScreenManager : IScreenManager,
        IRequestHandler<NewGameRequest>,
        IRequestHandler<ExitCurrentGameRequest>
    {
        private readonly TitleScreen _titleScreen;
        private readonly GameScreen _gameScreen;

        private Screen _activeScreen;

        public ScreenManager(
            TitleScreen titleScreen,
            GameScreen gameScreen
        )
        {
            _titleScreen = titleScreen;
            _gameScreen = gameScreen;
        }

        public void Initialize()
        {
            _titleScreen.Initialize();
            _gameScreen.Initialize();

            ShowScreen(_titleScreen);
        }

        public void Draw()
        {
            _activeScreen.Draw();
        }

        private void ShowScreen(Screen screen)
        {
            _activeScreen?.Hide();

            screen.Show();

            _activeScreen = screen;
        }

        public void Update()
        {
            _activeScreen.Update();
        }
        
        public Task<Unit> Handle(NewGameRequest request, CancellationToken cancellationToken)
        {
            ShowScreen(_gameScreen);
            _gameScreen.StartNewGame();

            return Unit.Task;
        }

        public Task<Unit> Handle(ExitCurrentGameRequest request, CancellationToken cancellationToken)
        {
            _gameScreen.EndGame();

            ShowScreen(_titleScreen);
            return Unit.Task;
        }
    }
}