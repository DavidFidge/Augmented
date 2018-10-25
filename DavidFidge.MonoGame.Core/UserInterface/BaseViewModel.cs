using DavidFidge.MonoGame.Core.Components;

namespace DavidFidge.MonoGame.Core.UserInterface
{
    public abstract class BaseViewModel<T> : BaseComponent
        where T : new()
    {
        public T Data { get; protected set; }

        public virtual void Initialize()
        {
            Data = new T();
        }
    }
}