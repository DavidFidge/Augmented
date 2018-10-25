namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface IMemento<T>
    {
        T State { get; set; }
    }
}