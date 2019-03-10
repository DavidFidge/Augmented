using System.Threading;
using System.Threading.Tasks;

using Augmented.Interfaces;
using Augmented.Messages;

using MediatR;

namespace Augmented.Graphics.Camera
{
    public class GameView3D : IRequestHandler<Zoom3DViewRequest>, IRequestHandler<Pan3DViewRequest>
    {
        private readonly IAugmentedGameWorld _augmentedGameWorld;

        public IGameCamera Camera { get; }

        public GameView3D(
            IGameCamera gameCamera,
            IAugmentedGameWorld augmentedGameWorld)
        {
            _augmentedGameWorld = augmentedGameWorld;
            Camera = gameCamera;
        }

        public void Initialise()
        {
            _augmentedGameWorld.LoadContent();
            Camera.Initialise();
        }

        public void Update()
        {
            Camera.Update();
        }

        public void Draw()
        {
            _augmentedGameWorld.Draw(Camera.View, Camera.Projection);
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