using System.Threading;
using System.Threading.Tasks;

using Augmented.Messages;
using Augmented.UserInterface.Data;
using Augmented.UserInterface.ViewModels;

using DavidFidge.MonoGame.Core.UserInterface;

using GeonBit.UI.Entities;

using MediatR;

using Microsoft.Xna.Framework;

namespace Augmented.UserInterface.Views
{
    public class GameView : BaseView<GameViewModel, GameData>,
        IRequestHandler<OpenInGameOptionsRequest>,
        IRequestHandler<CloseInGameOptionsRequest>
    {
        private readonly InGameOptionsView _inGameOptionsView;
        private readonly GameSpeedView _gameSpeedView;

        public GameView(
            GameViewModel gameViewModel,
            InGameOptionsView inGameOptionsView,
            GameSpeedView gameSpeedView
        )
            : base(gameViewModel)
        {
            _inGameOptionsView = inGameOptionsView;
            _gameSpeedView = gameSpeedView;
            _components.Add(_gameSpeedView);
        }

        protected override void InitializeInternal()
        {
            SetupInGameOptions();
            SetupGameSpeedView();
        }

        private void SetupGameSpeedView()
        {
            _gameSpeedView.Initialize();

            var timePanel = new Panel(
                new Vector2(300f, 110f),
                PanelSkin.Simple,
                Anchor.TopRight)
                .NoPadding();

            timePanel.Opacity = 50;

            _gameSpeedView.RootPanel.AddAsChildOf(timePanel);

            RootPanel.AddChild(timePanel);
        }

        private void SetupInGameOptions()
        {
            var menuButton = new Button(
                "-",
                ButtonSkin.Default,
                Anchor.TopLeft,
                new Vector2(50, 50))
                .SendOnClick<OpenInGameOptionsRequest>(Mediator)
                .NoPadding();

            RootPanel.AddChild(menuButton);

            _inGameOptionsView.Initialize();

            RootPanel.AddChild(_inGameOptionsView.RootPanel);
        }

        public Task<Unit> Handle(OpenInGameOptionsRequest request, CancellationToken cancellationToken)
        {
            _inGameOptionsView.Show();

            return Unit.Task;
        }

        public Task<Unit> Handle(CloseInGameOptionsRequest request, CancellationToken cancellationToken)
        {
            _inGameOptionsView.Hide();

            return Unit.Task;
        }

        public bool IsMouseIn3DView => RootPanel != null && RootPanel.IsMouseInRootPanelEmptySpace;
    }
}