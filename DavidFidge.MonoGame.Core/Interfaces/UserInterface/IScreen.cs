namespace DavidFidge.MonoGame.Core.Interfaces.UserInterface
{
    public interface IScreen
    {
        bool IsInitialized { get; }
        bool IsVisible { get; }
        void Show();
        void Initialize();
        void LoadContent();
        void UnloadContent();
        void Update();
        void Draw();
    }
}