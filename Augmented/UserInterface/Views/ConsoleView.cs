using System;
using System.Linq;
using System.Text;
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
        private Paragraph _consoleHistory;

        public ConsoleView(
            ConsoleViewModel consoleViewModel
        ) : base(consoleViewModel)
        {
        }

        protected override void InitializeInternal()
        {
            var containerPanel = new Panel(new Vector2(-1, 0.3f), PanelSkin.None, Anchor.BottomCenter, new Vector2(0f, -15f))
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

            _consoleHistory = new Paragraph(String.Empty, Anchor.AutoInline, new Vector2(-1, 1f));

            consolePanel.AddChild(_consoleHistory);
        }

        private void OnValueChange(Entity entity)
        {
            _consoleEntry.Value = _consoleEntry.Value.Replace("`", String.Empty);
        }

        public void FocusConsoleEntry()
        {
            _consoleEntry.IsFocused = true;
        }

        public override void Show()
        {
            base.Show();
            FocusConsoleEntry();
        }

        public Task<Unit> Handle(SendConsoleCommandRequest request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_consoleEntry.TextParagraph.Text))
            {
                var executeConsoleCommandRequest = new ExecuteConsoleCommandRequest(_consoleEntry.Value);

                _consoleEntry.Value = String.Empty;

                Mediator.Send(executeConsoleCommandRequest, cancellationToken);
            }

            return Unit.Task;
        }

        protected override void UpdateView()
        {
            var stringBuilder = new StringBuilder();

            foreach (var lastCommand in Data.LastCommands)
            {
                if (!string.IsNullOrEmpty(lastCommand.Result))
                    stringBuilder.AppendLine(lastCommand.Result);

                stringBuilder.AppendLine(lastCommand.Text);
            }

            _consoleHistory.Text = stringBuilder.ToString();

            base.UpdateView();
        }
    }
}