using GeonBit.UI.Entities;

using MediatR;

namespace DavidFidge.MonoGame.Core.UserInterface
{
    public static class ButtonExtensions
    {
        public static Button OnClick<T>(this Button button, IMediator mediator)
            where T : IRequest, new()
        {
            button.OnClick = entity => mediator.Send(new T());
            return button;
        }

        public static Button OnClick<T1, T2>(this Button button, IMediator mediator)
            where T1 : IRequest, new()
            where T2 : IRequest, new()
        {
            button.OnClick = entity =>
            {
                mediator.Send(new T1());
                mediator.Send(new T2());
            };

            return button;
        }

        public static Button AddTo(this Button button, Panel panel)
        {
            panel.AddChild(button);

            return button;
        }
    }
}
