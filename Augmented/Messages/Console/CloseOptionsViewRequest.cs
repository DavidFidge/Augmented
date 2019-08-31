using MediatR;

namespace Augmented.Messages
{
    public class ExecuteConsoleCommandRequest : IRequest
    {
        public ExecuteConsoleCommandRequest(string command)
        {
            Command = command;
        }

        public string Command { get; }
    }
}