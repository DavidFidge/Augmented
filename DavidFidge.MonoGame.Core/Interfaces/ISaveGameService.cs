namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface ISaveGameService
    {
        void LoadGame(ISaveGameStore saveGameStore);
        void SaveGame(ISaveGameStore saveGameStore);
    }
}