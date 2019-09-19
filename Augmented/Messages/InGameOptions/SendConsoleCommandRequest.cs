using DavidFidge.MonoGame.Core.UserInterface;

using MediatR;

using Microsoft.Xna.Framework.Input;

namespace Augmented.Messages
{
    [ActionMap(Name = "Send Console Command", DefaultKey = Keys.Enter)]
    public class SendConsoleCommandRequest : IRequest
    {
    }
}