namespace DavidFidge.MonoGame.Core.Interfaces.UserInterface
{
    public interface IView<T>
    {
        void Show();
        void Hide();
        void Initialize();

        IRootPanel<T> RootPanel { get; }
    }
}