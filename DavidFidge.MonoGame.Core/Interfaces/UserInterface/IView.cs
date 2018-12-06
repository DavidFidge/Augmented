namespace DavidFidge.MonoGame.Core.Interfaces.UserInterface
{
    public interface IView<T> : IView
    {
        IRootPanel<T> RootPanel { get; }
    }

    public interface IView
    {
        void Show();
        void Hide();
        void Initialize();
    }
}