using System.Threading;
using System.Threading.Tasks;

using Augmented.Messages;

using DavidFidge.MonoGame.Core.Interfaces;

using MediatR;

using Microsoft.Xna.Framework;

namespace Augmented
{
    public class GameView3D : IRequestHandler<Zoom3DViewRequest>, IRequestHandler<Pan3DViewRequest>
    {
        private readonly IGameProvider _gameProvider;
        private readonly IAugmentedGameWorld _augmentedGameWorld;

        public IGameCamera Camera { get; }

        public GameView3D(
            IGameCamera gameCamera,
            IGameProvider gameProvider,
            IAugmentedGameWorld augmentedGameWorld)
        {
            _gameProvider = gameProvider;
            _augmentedGameWorld = augmentedGameWorld;
            Camera = gameCamera;
        }

        public void Initialise()
        {
            _augmentedGameWorld.Initialise();
            Camera.Initialise();
        }

        public void Update()
        {
            Camera.Update();
        }

        public void Draw()
        {
            _augmentedGameWorld.Draw(Camera.Projection, Camera.View);
        }

        public Task<Unit> Handle(Zoom3DViewRequest request, CancellationToken cancellationToken)
        {
            Camera.Zoom(request.Difference);
            return Unit.Task;
        }

        public Task<Unit> Handle(Pan3DViewRequest request, CancellationToken cancellationToken)
        {
            Camera.GameUpdateContinuousMovement = request.CameraMovementFlags;
            return Unit.Task;
        }
    }
}