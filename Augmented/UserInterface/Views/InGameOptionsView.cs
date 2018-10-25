using Augmented.Messages;
using Augmented.UserInterface.Data;
using Augmented.UserInterface.ViewModels;

using DavidFidge.MonoGame.Core.UserInterface;

using GeonBit.UI.Entities;

using Microsoft.Xna.Framework;

namespace Augmented.UserInterface.Views
{
    public class InGameOptionsView : BaseView<InGameOptionsViewModel, InGameOptionsData>
    {
        private Panel _inGameOptionsMenuPanel;

        public InGameOptionsView(
            InGameOptionsViewModel inGameOptionsViewModel
        ) : base(inGameOptionsViewModel)
        {
        }

        protected override void InitializeInternal()
        {
            _inGameOptionsMenuPanel = new Panel(new Vector2(500, 400));

            RootPanel.AddChild(_inGameOptionsMenuPanel);

            var headingLabel = new Label(Data.Heading, Anchor.AutoCenter)
                .H4Heading();

            _inGameOptionsMenuPanel.AddChild(headingLabel);

            var line = new HorizontalLine(Anchor.AutoCenter);

            _inGameOptionsMenuPanel.AddChild(line);

            new Button("Exit Game")
                .OnClick<CloseInGameOptionsRequest, ExitCurrentGameRequest>(Mediator)
                .AddTo(_inGameOptionsMenuPanel);

            new Button("Back to Game")
                .OnClick<CloseInGameOptionsRequest>(Mediator)
                .AddTo(_inGameOptionsMenuPanel);
        }
    }
}