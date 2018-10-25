using DavidFidge.MonoGame.Core.Services;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface IGameTimeService : ISaveable
    {
        void Reset();
        void Update(GameTime gameTime);
        void PauseGame();
        void ResumeGame();
        void IncreaseGameSpeed();

        GameTime OriginalGameTime { get; }
        CustomGameTime GameTime { get; }
    }
}