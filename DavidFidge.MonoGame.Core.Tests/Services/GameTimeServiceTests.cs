using System;

using DavidFidge.MonoGame.Core.Interfaces;
using DavidFidge.MonoGame.Core.Services;
using DavidFidge.TestInfrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

using NSubstitute;

namespace DavidFidge.MonoGame.Core.Tests.Services
{
    [TestClass]
    public class GameTimeServiceTests : BaseTest
    {
        private GameTimeService _gameTimeService;
        private FakeStopwatchProvider _fakeStopwatchProvider;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _fakeStopwatchProvider = new FakeStopwatchProvider();
            _gameTimeService = new GameTimeService(_fakeStopwatchProvider);
        }

        [TestMethod]
        public void Reset_Should_Reset_GameTime()
        {
            // Arrange
            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(1);

            _gameTimeService.IncreaseGameSpeed();
            _gameTimeService.PauseGame();
            _gameTimeService.Update(new GameTime());

            // Act
            _gameTimeService.Reset();

            // Assert
            Assert.AreEqual(TimeSpan.Zero, _fakeStopwatchProvider.Elapsed);
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.TotalRealTime);
        }

        [TestMethod]
        public void LoadGame_Should_Load_GameTime_Data_From_Memento()
        {
            // Arrange
            var memento = new Memento<GameTimeServiceSaveData>(
                new GameTimeServiceSaveData
                {
                    TotalGameTime = TimeSpan.FromSeconds(2)
                });

            var saveGameStore = Substitute.For<ISaveGameStore>();
            saveGameStore.GetFromStore<GameTimeServiceSaveData>().Returns(memento);

            // Act
            _gameTimeService.LoadGame(saveGameStore);

            // Assert
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(2), _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.TotalRealTime);
        }

        [TestMethod]
        public void SaveGame_Should_Return_Memento_With_GameTime()
        {
            // Arrange
            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(2);
            _gameTimeService.Update(new GameTime());

            var saveGameStore = Substitute.For<ISaveGameStore>();

            // Act
            _gameTimeService.SaveGame(saveGameStore);


            // Assert
            saveGameStore
                .Received()
                .SaveToStore(
                    Arg.Is<IMemento<GameTimeServiceSaveData>>(m => 
                        m.State.TotalGameTime == _gameTimeService.GameTime.TotalGameTime
                    )
                );
        }

        [TestMethod]
        public void Update_Should_Advance_GameTime()
        {
            // Arrange
            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(1);

            // Act
            _gameTimeService.Update(new GameTime());

            // Assert
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.TotalRealTime);
        }

        [TestMethod]
        public void Update_Should_Advance_GameTime_By_Elapsed_Amount_Given_2_DIfferent_Updates()
        {
            // Arrange
            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(1);

            // Act
            _gameTimeService.Update(new GameTime());

            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(3);

            _gameTimeService.Update(new GameTime());

            // Assert
            Assert.AreEqual(TimeSpan.FromSeconds(2), _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(2), _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.FromSeconds(3), _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(3), _gameTimeService.GameTime.TotalRealTime);
        }

        [TestMethod]
        public void PauseGame_Should_Not_Advance_GameTime_When_Updates_Occur()
        {
            // Arrange
            _gameTimeService.PauseGame();

            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(1);

            // Act
            _gameTimeService.Update(new GameTime());

            // Assert
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.TotalRealTime);
            Assert.IsTrue(_gameTimeService.IsPaused);
        }

        [TestMethod]
        public void ResumeGame_Should_Advance_GameTime_When_Updates_Occur()
        {
            // Arrange
            _gameTimeService.PauseGame();

            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(1);

            _gameTimeService.Update(new GameTime());

            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(3);

            // Act
            _gameTimeService.ResumeGame();
            _gameTimeService.Update(new GameTime());

            // Assert
            Assert.AreEqual(TimeSpan.FromSeconds(2), _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(2), _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.FromSeconds(2), _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(3), _gameTimeService.GameTime.TotalRealTime);
            Assert.IsFalse(_gameTimeService.IsPaused);
        }

        [TestMethod]
        [DataRow(1, 1250)]
        [DataRow(2, 1500)]
        [DataRow(11, 3750)]
        [DataRow(12, 4000)]
        [DataRow(13, 4000)]  //// Limited to 4x real time by default
        public void IncreaseGameSpeed_Should_Advance_GameTime_At_Faster_Rate_Than_RealTime(
            int numberOfTimes,
            double expectedMilliseconds)
        {
            // Arrange
            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(1);

            // Act
            for (int i = 0; i < numberOfTimes; i++)
                _gameTimeService.IncreaseGameSpeed();

            _gameTimeService.Update(new GameTime());

            // Assert
            Assert.AreEqual(TimeSpan.FromMilliseconds(expectedMilliseconds), _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.FromMilliseconds(expectedMilliseconds), _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.TotalRealTime);
        }

        [TestMethod]
        [DataRow(1, 750)]
        [DataRow(2, 500)]
        [DataRow(3, 250)]
        public void DecreaseGameSpeed_Should_Advance_GameTime_At_Slower_Rate_Than_RealTime(
            int numberOfTimes,
            double expectedMilliseconds)
        {
            // Arrange
            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(1);

            // Act
            for (int i = 0; i < numberOfTimes; i++)
                _gameTimeService.DecreaseGameSpeed();

            _gameTimeService.Update(new GameTime());

            // Assert
            Assert.AreEqual(TimeSpan.FromMilliseconds(expectedMilliseconds), _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.FromMilliseconds(expectedMilliseconds), _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.TotalRealTime);
        }


        [TestMethod]
        public void DecreaseGameSpeed_Should_Pause_Game_At_Slowest_Rate()
        {
            // Arrange
            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(1);

            // Act
            for (int i = 0; i < 4; i++)
                _gameTimeService.DecreaseGameSpeed();

            _gameTimeService.Update(new GameTime());

            // Assert
            Assert.IsTrue(_gameTimeService.IsPaused);
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.Zero, _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.TotalRealTime);
        }


        [TestMethod]
        public void IncreaseGameSpeed_Should_Resume_Game_At_Same_Speed_When_Paused()
        {
            // Arrange
            _fakeStopwatchProvider.Elapsed = TimeSpan.FromSeconds(1);

            // Act
            _gameTimeService.PauseGame();
            _gameTimeService.IncreaseGameSpeed();
            _gameTimeService.Update(new GameTime());

            // Assert
            Assert.IsFalse(_gameTimeService.IsPaused);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.ElapsedGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.TotalGameTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.ElapsedRealTime);
            Assert.AreEqual(TimeSpan.FromSeconds(1), _gameTimeService.GameTime.TotalRealTime);
        }
    }
}
