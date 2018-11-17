using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Interfaces
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