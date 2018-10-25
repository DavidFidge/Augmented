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

        public GameView(
            GameViewModel gameViewModel,
            InGameOptionsView inGameOptionsView
        )
            : base(gameViewModel)
        {
            _inGameOptionsView = inGameOptionsView;
        }

        protected override void InitializeInternal()
        {
            SetupInGameOptions();
        }

        private void SetupInGameOptions()
        {
            var menuButton = new Button(
                "-",
                ButtonSkin.Default,
                Anchor.TopLeft,
                new Vector2(50, 50)
            ).OnClick<OpenInGameOptionsRequest>(Mediator);

            menuButton.Padding = new Vector2(0, 0);

            RootPanel.AddChild(menuButton);

            _inGameOptionsView.Initialize();

            RootPanel.AddNestedRoot(_inGameOptionsView.RootPanel);
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
    }
}