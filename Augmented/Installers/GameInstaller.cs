using Augmented.Graphics;
using Augmented.Graphics.Camera;
using Augmented.Graphics.Models;
using Augmented.Graphics.TerrainSpace;
using Augmented.Interfaces;
using Augmented.Messages;
using Augmented.UserInterface.Data;
using Augmented.UserInterface.Input;
using Augmented.UserInterface.Screens;
using Augmented.UserInterface.ViewModels;
using Augmented.UserInterface.Views;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using DavidFidge.MonoGame.Core.Graphics.Cylinder;
using DavidFidge.MonoGame.Core.Graphics.Terrain;
using DavidFidge.MonoGame.Core.Graphics.Trees;
using DavidFidge.MonoGame.Core.Installers;
using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;
using DavidFidge.MonoGame.Core.Messages;

using InputHandlers.Keyboard;
using InputHandlers.Mouse;

using MediatR;

namespace Augmented.Installers
{
    public class GameInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(new CoreInstaller());

            RegisterTitleView(container, store);
            RegisterOptionsView(container, store);
            RegisterVideoOptionsView(container, store);
            RegisterInGameOptionsView(container, store);
            RegisterGameView(container, store);
            RegisterGameSpeedView(container, store);

            container.Register(

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<NullKeyboardHandler>()
                    .IsDefault(),

                Component.For<IMouseHandler>()
                    .ImplementedBy<NullMouseHandler>()
                    .IsDefault(),

                Component.For<IGame>()
                    .Forward<IRequestHandler<ExitGameRequest, Unit>>()
                    .ImplementedBy<AugmentedGame>(),

                Component.For<IScreenManager>()
                    .Forward<IRequestHandler<NewGameRequest, Unit>>()
                    .Forward<IRequestHandler<ExitCurrentGameRequest, Unit>>()
                    .ImplementedBy<ScreenManager>(),

                Component.For<TitleScreen>(),
                Component.For<GameScreen>(),

                Component.For<GameView3D>()
                    .Forward<IRequestHandler<Pan3DViewRequest, Unit>>()
                    .Forward<IRequestHandler<Zoom3DViewRequest, Unit>>()
                    .Forward<IRequestHandler<Rotate3DViewRequest, Unit>>()
                    .DependsOn(Dependency.OnComponent<IGameCamera, FreeGameCamera>()),

                Component.For<IHeightMapGenerator>()
                    .ImplementedBy<HeightMapGenerator>(),

                Component.For<Terrain>()
                    .LifeStyle.Transient,

                Component.For<AugmentedModel>()
                    .LifeStyle.Transient,

                Component.For<Cylinder>()
                    .LifeStyle.Transient,

                Component.For<Tree>()
                    .LifeStyle.Transient,

                Component.For<IGameCamera>()
                    .ImplementedBy<FreeGameCamera>()
                    .LifeStyle.Transient,

                Component.For<IGameCamera>()
                    .ImplementedBy<StrategyGameCamera>()
                    .LifeStyle.Transient,

                Component.For<IAugmentedGameWorld>()
                    .ImplementedBy<AugmentedGameWorld>()
                    .LifeStyle.Transient
            );
        }

        private void RegisterTitleView(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<TitleView>()
                    .Forward<IRequestHandler<OptionsButtonClickedRequest, Unit>>()
                    .Forward<IRequestHandler<CloseOptionsViewRequest, Unit>>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, TitleViewKeyboardHandler>()),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<TitleViewKeyboardHandler>(),

                Component.For<TitleViewModel>());
        }

        private void RegisterOptionsView(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<OptionsView>()
                    .Forward<IRequestHandler<OpenVideoOptionsRequest, Unit>>()
                    .Forward<IRequestHandler<CloseVideoOptionsRequest, Unit>>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, OptionsKeyboardHandler>()),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<OptionsKeyboardHandler>(),

                Component.For<OptionsViewModel>()
            );
        }

        private void RegisterVideoOptionsView(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(

                Component.For<VideoOptionsViewModel>()
                    .Forward<IRequestHandler<SetDisplayModeRequest, Unit>>()
                    .Forward<IRequestHandler<SaveVideoOptionsRequest, Unit>>()
                    .Forward<IRequestHandler<VideoOptionsFullScreenToggle, Unit>>()
                    .Forward<IRequestHandler<VideoOptionsVerticalSyncToggle, Unit>>()
                    .ImplementedBy<VideoOptionsViewModel>(),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<VideoOptionsKeyboardHandler>(),

                Component.For<VideoOptionsView>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, VideoOptionsKeyboardHandler>())
            );
        }

        private void RegisterGameView(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<GameView>()
                    .Forward<IRequestHandler<OpenInGameOptionsRequest, Unit>>()
                    .Forward<IRequestHandler<CloseInGameOptionsRequest, Unit>>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, GameViewKeyboardHandler>())
                    .DependsOn(Dependency.OnComponent<IMouseHandler, GameViewMouseHandler>()),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<GameViewKeyboardHandler>(),

                Component.For<IMouseHandler>()
                    .ImplementedBy<GameViewMouseHandler>(),

                Component.For<GameViewModel>()
            );
        }

        private void RegisterInGameOptionsView(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<InGameOptionsView>()
                    .ImplementedBy<InGameOptionsView>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, InGameOptionsKeyboardHandler>()),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<InGameOptionsKeyboardHandler>(),

                Component.For<InGameOptionsViewModel>()
            );
        }

        private void RegisterGameSpeedView(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<GameSpeedView>()
                    .Forward<IRequestHandler<UpdateViewRequest<GameSpeedData>, Unit>>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, GameSpeedKeyboardHandler>()),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<GameSpeedKeyboardHandler>(),

                Component.For<GameSpeedViewModel>()
                    .Forward<INotificationHandler<GameTimeUpdateNotification>>()
                    .Forward<IRequestHandler<ChangeGameSpeedRequest, Unit>>());
        }
    }
}
