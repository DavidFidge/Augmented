namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface ISaveable
    {
        void SaveGame(ISaveGameStore saveGameStore);
        void LoadGame(ISaveGameStore saveGameStore);
    }
}