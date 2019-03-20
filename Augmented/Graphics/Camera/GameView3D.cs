using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Augmented.Interfaces;
using Augmented.Messages;
using DavidFidge.MonoGame.Core.Graphics.Camera;
using MediatR;

namespace Augmented.Graphics.Camera
{
    public class GameView3D :
        IRequestHandler<Zoom3DViewRequest>,
        IRequestHandler<Move3DViewRequest>,
        IRequestHandler<Rotate3DViewRequest>
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

        public Task<Unit> Handle(Move3DViewRequest request, CancellationToken cancellationToken)
        {
            Camera.GameUpdateContinuousMovement = request.CameraMovementFlags;

            return Unit.Task;
        }

        public Task<Unit> Handle(Rotate3DViewRequest request, CancellationToken cancellationToken)
        {
            if (request.XRotation > float.Epsilon)
                Camera.Rotate(CameraMovement.RotateDown, request.XRotation);
            else if (request.XRotation < float.Epsilon)
                Camera.Rotate(CameraMovement.RotateUp, -request.XRotation);

            if (request.ZRotation > float.Epsilon)
                Camera.Rotate(CameraMovement.RotateLeft, request.ZRotation);
            else if (request.ZRotation < float.Epsilon)
                Camera.Rotate(CameraMovement.RotateRight, -request.ZRotation);

            return Unit.Task;
        }
    }
}