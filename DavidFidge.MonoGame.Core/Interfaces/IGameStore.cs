namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface IGameStore
    {
        IMemento<T> GetFromStore<T>();
        void SaveToStore<T>(IMemento<T> memento);
    }
}