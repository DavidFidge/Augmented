using Castle.Core.Logging;

using MediatR;

namespace DavidFidge.MonoGame.Core.Components
{
    public abstract class BaseComponent
    {
        public IMediator Mediator { get; set; }
        public ILogger Logger { get; set; }
    }
}