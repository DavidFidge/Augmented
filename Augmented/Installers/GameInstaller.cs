using Augmented.Graphics;
using Augmented.Graphics.Camera;
using Augmented.Interfaces;
using Augmented.Messages;
using Augmented.UserInterface.Input;
using Augmented.UserInterface.Screens;
using Augmented.UserInterface.ViewModels;
using Augmented.UserInterface.Views;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.MonoGame.Core.Installers;
using DavidFidge.MonoGame.Core.Interfaces.Components;
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

            container.Register(

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<NullKeyboardHandler>(),

                Component.For<IMouseHandler>()
                    .ImplementedBy<NullMouseHandler>(),

                Component.For<IGame>()
                    .Forward<IRequestHandler<ExitGameRequest, Unit>>()
                    .ImplementedBy<AugmentedGame>(),

                Component.For<IScreenManager>()
                    .Forward<IRequestHandler<NewGameRequest, Unit>>()
                    .Forward<IRequestHandler<ExitCurrentGameRequest, Unit>>()
                    .ImplementedBy<ScreenManager>(),

                Component.For<TitleScreen>(),
                Component.For<GameScreen>(),

                Component.For<TitleView>()
                    .Forward<IRequestHandler<OptionsButtonClickedRequest, Unit>>()
                    .Forward<IRequestHandler<CloseOptionsViewRequest, Unit>>()
                    .ImplementedBy<TitleView>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, TitleViewKeyboardHandler>()),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<TitleViewKeyboardHandler>(),

                Component.For<TitleViewModel>(),

                Component.For<OptionsView>()
                    .Forward<IRequestHandler<OpenVideoOptionsRequest, Unit>>()
                    .Forward<IRequestHandler<CloseVideoOptionsRequest, Unit>>()
                    .ImplementedBy<OptionsView>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, OptionsKeyboardHandler>()),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<OptionsKeyboardHandler>(),

                Component.For<OptionsViewModel>(),

                Component.For<VideoOptionsViewModel>()
                    .Forward<IRequestHandler<SetDisplayModeRequest, Unit>>()
                    .Forward<IRequestHandler<SaveVideoOptionsRequest, Unit>>()
                    .Forward<IRequestHandler<VideoOptionsFullScreenToggle, Unit>>()
                    .Forward<IRequestHandler<VideoOptionsVerticalSyncToggle, Unit>>()
                    .ImplementedBy<VideoOptionsViewModel>(),
            
                Component.For<IKeyboardHandler>()
                    .ImplementedBy<VideoOptionsKeyboardHandler>(),

                Component.For<VideoOptionsView>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, VideoOptionsKeyboardHandler>()),

                Component.For<GameView>()
                    .Forward<IRequestHandler<OpenInGameOptionsRequest, Unit>>()
                    .Forward<IRequestHandler<CloseInGameOptionsRequest, Unit>>()
                    .ImplementedBy<GameView>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, GameViewKeyboardHandler>())
                    .DependsOn(Dependency.OnComponent<IMouseHandler, GameViewMouseHandler>()),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<GameViewKeyboardHandler>(),

                Component.For<IMouseHandler>()
                    .ImplementedBy<GameViewMouseHandler>(),

                Component.For<GameViewModel>(),

                Component.For<InGameOptionsView>()
                    .ImplementedBy<InGameOptionsView>()
                    .DependsOn(Dependency.OnComponent<IKeyboardHandler, InGameOptionsKeyboardHandler>()),

                Component.For<IKeyboardHandler>()
                    .ImplementedBy<InGameOptionsKeyboardHandler>(),

                Component.For<InGameOptionsViewModel>(),

                Component.For<GameView3D>()
                    .Forward<IRequestHandler<Pan3DViewRequest, Unit>>()
                    .Forward<IRequestHandler<Zoom3DViewRequest, Unit>>(),

                Component.For<TestQuad>()
                    .LifeStyle.Transient,

                Component.For<MaterialQuadTemplate>()
                    .LifeStyle.Transient,

                Component.For<IGameCamera>()
                    .ImplementedBy<GameCamera>()
                    .LifeStyle.Transient,

                Component.For<IAugmentedGameWorld>()
                    .ImplementedBy<AugmentedGameWorld>()
                    .LifeStyle.Transient
            );
        }
    }
}
