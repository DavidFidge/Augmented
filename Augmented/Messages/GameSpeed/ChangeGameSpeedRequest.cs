using MediatR;

namespace Augmented.Messages
{
    public class ChangeGameSpeedRequest : IRequest
    {
        public int Increment { get; private set; }
        public bool TogglePauseGame { get; private set; }
        public bool Reset { get; private set; }

        public ChangeGameSpeedRequest ResetRequest()
        {
            Reset = true;
            return this;
        }

        public ChangeGameSpeedRequest TogglePauseGameRequest()
        {
            TogglePauseGame = true;
            return this;
        }

        public ChangeGameSpeedRequest IncreaseSpeedRequest()
        {
            Increment = 1;
            return this;
        }

        public ChangeGameSpeedRequest DecreaseSpeedRequest()
        {
            Increment = -1;
            return this;
        }
    }
}