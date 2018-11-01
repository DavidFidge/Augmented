using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Augmented.UserInterface.Data;
using Augmented.UserInterface.Screens;

using DavidFidge.MonoGame.Core.Interfaces;
using DavidFidge.MonoGame.Core.Messages;
using DavidFidge.MonoGame.Core.Services;

using MediatR;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Serilog;

namespace Augmented
{
    public class AugmentedGame : Game, IGame
    {
        private readonly ILogger _logger;
        private readonly IGameProvider _gameProvider;
        private readonly IGameTimeService _gameTimeService;
        private readonly IGameInputService _gameInputService;
        private readonly IUserInterface _userInterface;
        private readonly IGameOptionsStore _gameOptionsStore;
        private readonly IScreenManager _screenManager;
        private bool _isExiting;

        public CustomGraphicsDeviceManager CustomGraphicsDeviceManager { get; }
        private SpriteBatch _spriteBatch;

        public AugmentedGame(
            ILogger logger,
            IGameProvider gameProvider,
            IGameTimeService gameTimeService,
            IGameInputService gameInputService,
            IUserInterface userInterface,
            IGameOptionsStore gameOptionsStore,
            IScreenManager screenManager
            )
        {
            _logger = logger;
            _gameProvider = gameProvider;
            _gameProvider.Game = this;
            _gameTimeService = gameTimeService;
            _gameInputService = gameInputService;
            _userInterface = userInterface;
            _gameOptionsStore = gameOptionsStore;
            _logger.Debug("Starting game");

            CustomGraphicsDeviceManager = new CustomGraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _screenManager = screenManager;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _userInterface.Initialize(Content);

            // make the window fullscreen (but still with border and top control bar)
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InitializeDisplaySettings();

            _screenManager.Initialize();

            base.Initialize();
        }

        private void InitializeDisplaySettings()
        {
            var screenWidth = CustomGraphicsDeviceManager.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            var screenHeight = CustomGraphicsDeviceManager.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            var isFullScreen = false;
            var isVerticalSync = true;

            var gameOptions = _gameOptionsStore.GetFromStore<VideoOptionsData>()?.State;

            if (gameOptions != null && gameOptions.SelectedDisplayMode != null)
            {
                var displayModes = CustomGraphicsDeviceManager.GetSupportedDisplayModes();

                if (displayModes.Any(dm => Equals(dm, gameOptions.SelectedDisplayMode)))
                {
                    screenWidth = gameOptions.SelectedDisplayMode.Width;
                    screenHeight = gameOptions.SelectedDisplayMode.Height;
                    isFullScreen = gameOptions.IsFullScreen;
                    isVerticalSync = gameOptions.IsVerticalSync;
                }
            }

            CustomGraphicsDeviceManager.PreferredBackBufferWidth = screenWidth;
            CustomGraphicsDeviceManager.PreferredBackBufferHeight = screenHeight;
            CustomGraphicsDeviceManager.IsFullScreen = isFullScreen;
            CustomGraphicsDeviceManager.IsVerticalSync = isVerticalSync;
            CustomGraphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (_isExiting)
                Exit();

            _userInterface.Update(gameTime);

            _gameTimeService.Update(gameTime);
            _gameInputService.Poll();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // draw ui
            _userInterface.Draw(_spriteBatch);

            // clear buffer
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _screenManager.Initialize();

            // finalize ui rendering

            _userInterface.DrawMainRenderTarget(_spriteBatch);

            //_spriteBatch.End();
            base.Draw(gameTime);
        }

        public Task<Unit> Handle(ExitGameRequest request, CancellationToken cancellationToken)
        {
            _isExiting = true;
            return Unit.Task;
        }
    }
}
