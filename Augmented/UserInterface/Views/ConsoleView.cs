using System;
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
    public class ConsoleView : BaseView<ConsoleViewModel, ConsoleData>,
        IRequestHandler<SendConsoleCommandRequest>
    {
        private TextInput _consoleEntry;

        public ConsoleView(
            ConsoleViewModel consoleViewModel
        ) : base(consoleViewModel)
        {
        }

        protected override void InitializeInternal()
        {
            var containerPanel = new Panel(new Vector2(-1, 0.25f), PanelSkin.None, Anchor.BottomCenter, new Vector2(0f, 20f))
            {
                Padding = new Vector2(0f, 0f),
            };

            RootPanel.AddChild(containerPanel);

            var consolePanel = new Panel(new Vector2(-1, -1))
            {
                Padding = new Vector2(30f, 30f)
            };

            containerPanel.AddChild(consolePanel);

            var consolePrompt = new Label(">", Anchor.AutoInlineNoBreak, new Vector2(0.03f, 0.2f))
            {
                TextStyle = FontStyle.Bold,
                Scale = 1.2f
            };

            consolePanel.AddChild(consolePrompt);

            _consoleEntry = new TextInput(false, new Vector2(0.96f, 0.2f), Anchor.AutoInlineNoBreak, null, PanelSkin.Simple)
            {
                Padding = new Vector2(0.1f, 0f),
                FillColor = Color.Black,
                Opacity = 128
            };

            _consoleEntry.OnValueChange = OnValueChange;

            consolePanel.AddChild(_consoleEntry);

            var hr = new HorizontalLine(Anchor.AutoInline);

            consolePanel.AddChild(hr);

            var consoleHistory = new Paragraph("xxx yyy", Anchor.AutoInline, new Vector2(-1, 0.8f));

            consolePanel.AddChild(consoleHistory);
        }

        private void OnValueChange(Entity entity)
        {
            _consoleEntry.TextParagraph.Text.Replace("`", String.Empty);
        }

        public void FocusConsoleEntry()
        {
            _consoleEntry.IsFocused = true;
        }

        public void ExecuteCommand()
        {
        }

        public override void Show()
        {
            base.Show();
            FocusConsoleEntry();
        }

        public Task<Unit> Handle(SendConsoleCommandRequest request, CancellationToken cancellationToken)
        {
            
            return Unit.Task;
        }
    }
}