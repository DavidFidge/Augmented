using System;

using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Services;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Services
{
    public class GameTimeService : IGameTimeService
    {
        private TimeSpan _lastElapsedRealTime;
        private readonly IStopwatchProvider _realTimeStopwatch;

        public GameTimeService(IStopwatchProvider stopwatchProvider)
        {
            _realTimeStopwatch = stopwatchProvider;
            _lastElapsedRealTime = new TimeSpan(0);

            GameTime = new CustomGameTime();
            GameTime.ElapsedGameTime = new TimeSpan(0);
            GameTime.ElapsedRealTime = new TimeSpan(0);
            GameTime.TotalGameTime = new TimeSpan(0);
            GameTime.TotalRealTime = new TimeSpan(0);
            GameTime.IsRunningSlowly = false;
            GameSpeedPercent = 100;
            IsPaused = false;
            _realTimeStopwatch.Start();
        }

        public CustomGameTime GameTime { get; private set; }
        public GameTime OriginalGameTime { get; private set; }

        public int GameSpeedPercent { get; private set; }
        public bool IsPaused { get; private set; }

        public void Reset()
        {
            Initialise();

            _realTimeStopwatch.Restart();
        }

        private void Initialise()
        {
            OriginalGameTime = null;
            GameTime.ElapsedGameTime = new TimeSpan(0);
            GameTime.ElapsedRealTime = new TimeSpan(0);
            GameTime.TotalGameTime = new TimeSpan(0);
            GameTime.TotalRealTime = new TimeSpan(0);
            GameTime.IsRunningSlowly = false;
            GameSpeedPercent = 100;
            IsPaused = false;
            _lastElapsedRealTime = new TimeSpan(0);
        }

        public void Update(GameTime gameTime)
        {
            OriginalGameTime = gameTime;

            var elapsedRealTime = _realTimeStopwatch.Elapsed;

            GameTime.ElapsedRealTime = elapsedRealTime - _lastElapsedRealTime;
            _lastElapsedRealTime = elapsedRealTime;
            GameTime.TotalRealTime = elapsedRealTime;

            if (IsPaused)
            {
                GameTime.ElapsedGameTime = new TimeSpan(0);
                return;
            }

            GameTime.ElapsedGameTime = TimeSpan.FromTicks(GameTime.ElapsedRealTime.Ticks * GameSpeedPercent / 100);
            GameTime.TotalGameTime = GameTime.TotalGameTime.Add(GameTime.ElapsedGameTime);
        }

        public void PauseGame()
        {
            IsPaused = true;
        }

        public void ResumeGame()
        {
            IsPaused = false;
        }

        public void IncreaseGameSpeed()
        {
            if (IsPaused)
                ResumeGame();

            else if (GameSpeedPercent < 400)
                GameSpeedPercent += 25;
        }

        public void DecreaseGameSpeed()
        {
            if (GameSpeedPercent > 25)
                GameSpeedPercent -= 25;
            else
                PauseGame();
        }

        public void SaveGame(ISaveGameStore saveGameStore)
        {
            var memento = new Memento<GameTimeServiceSaveData>(
                new GameTimeServiceSaveData
                {
                    TotalGameTime = GameTime.TotalGameTime
                });

            saveGameStore.SaveToStore(memento);
        }

        public void LoadGame(ISaveGameStore saveGameStore)
        {
            Initialise();

            var gameTimeServiceData = saveGameStore.GetFromStore<GameTimeServiceSaveData>();

            GameTime.TotalGameTime = gameTimeServiceData.State.TotalGameTime;

            _realTimeStopwatch.Restart();
        }
    }

    public class CustomGameTime : GameTime
    {
        public TimeSpan ElapsedRealTime { get; set; }
        public TimeSpan TotalRealTime { get; set; }
    }
}