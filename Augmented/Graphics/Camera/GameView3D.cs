using System;
using System.Threading;
using System.Threading.Tasks;

using Augmented.Interfaces;
using Augmented.Messages;
using DavidFidge.MonoGame.Core.Graphics.Camera;
using MediatR;

namespace Augmented.Graphics.Camera
{
    public class GameView3D :
        IRequestHandler<Pick3DViewRequest>,
        IRequestHandler<Action3DViewRequest>,
        IRequestHandler<Move3DViewRequest>,
        IRequestHandler<Rotate3DViewRequest>
    {
        private readonly IAugmentedGameWorld _augmentedGameWorld;
        private readonly IGameCamera _camera;

        public GameView3D(
            IGameCamera gameCamera,
            IAugmentedGameWorld augmentedGameWorld)
        {
            _augmentedGameWorld = augmentedGameWorld;
            _camera = gameCamera;
        }

        public void Initialise()
        {
            _camera.Initialise();
        }

        public void Update()
        {
            _camera.Update();
        }

        public void Draw()
        {
            _augmentedGameWorld.Draw(_camera.View, _camera.Projection);
        }

        public Task<Unit> Handle(Zoom3DViewRequest request, CancellationToken cancellationToken)
        {
            _camera.Zoom(request.Difference);
            return Unit.Task;
        }

        public Task<Unit> Handle(Move3DViewRequest request, CancellationToken cancellationToken)
        {
            _camera.GameUpdateContinuousMovement = request.CameraMovementFlags;

            return Unit.Task;
        }

        public Task<Unit> Handle(Rotate3DViewRequest request, CancellationToken cancellationToken)
        {
            if (request.XRotation > float.Epsilon)
                _camera.Rotate(CameraMovement.RotateDown, request.XRotation);
            else if (request.XRotation < float.Epsilon)
                _camera.Rotate(CameraMovement.RotateUp, -request.XRotation);

            if (request.ZRotation > float.Epsilon)
                _camera.Rotate(CameraMovement.RotateLeft, request.ZRotation);
            else if (request.ZRotation < float.Epsilon)
                _camera.Rotate(CameraMovement.RotateRight, -request.ZRotation);

            return Unit.Task;
        }

        public Task<Unit> Handle(Pick3DViewRequest request, CancellationToken cancellationToken)
        {
            var ray = _camera.GetPointerRay(request.X, request.Y);

            _augmentedGameWorld.Pick(ray);

            return Unit.Task;
        }

        public Task<Unit> Handle(Action3DViewRequest request, CancellationToken cancellationToken)
        {
            var ray = _camera.GetPointerRay(request.X, request.Y);

            _augmentedGameWorld.Action(ray);

            return Unit.Task;
        }
    }
}