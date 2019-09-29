using MediatR;

namespace Augmented.Messages
{
    public class Pick3DViewRequest : IRequest
    {
        public Pick3DViewRequest(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
}