using Augmented.Interfaces;

using DavidFidge.MonoGame.Core.Components;
using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;

using IDrawable = DavidFidge.MonoGame.Core.Graphics.IDrawable;

namespace Augmented.Graphics.Models
{
    public class AugmentedEntity : Entity, IDrawable, ISelectable, IActionable
    {
        protected readonly IGameProvider _gameProvider;
        private readonly IAugmentedModelDrawer _augmentedModelDrawer;
        private readonly ISelectionModelDrawer _selectionModelDrawer;
        private readonly StateMachine<AugmentedEntity> _stateMachine;
        private readonly MovingState _movingState;
        private readonly IdleState _idleState;

        public bool IsSelected { get; set; }
        public bool IsTargeted { get; set; }

        public AugmentedEntity(
            IGameProvider gameProvider,
            IAugmentedModelDrawer augmentedModelDrawer,
            ISelectionModelDrawer selectionModelDrawer)
        {
            _stateMachine = new StateMachine<AugmentedEntity>(this);
            _movingState = new MovingState();
            _idleState = new IdleState();

            _gameProvider = gameProvider;
            _augmentedModelDrawer = augmentedModelDrawer;
            _selectionModelDrawer = selectionModelDrawer;

            _selectionModelDrawer.BoundingBox = _augmentedModelDrawer.BoundingBox;

            LocalTransform.ChangeTranslation(new Vector3(0, 0, _augmentedModelDrawer.BoundingBox.Max.Z - _augmentedModelDrawer.BoundingBox.Min.Z) / 2f);

        }

        public void Draw(Matrix view, Matrix projection, Matrix world)
        {
            if (IsSelected)
                _selectionModelDrawer.Draw(view, projection, world);

            _augmentedModelDrawer.Draw(view, projection, world);
        }

        public float? Intersects(Ray ray, Matrix worldTransform)
        {
            return ray.Intersects(_augmentedModelDrawer.BoundingSphere.Transform(worldTransform));
        }

        public void MoveTo(Vector3 target)
        {
            _movingState.Target = target;
            _stateMachine.ChangeState(_movingState);
        }

        public class MovingState : State<AugmentedEntity>
        {
            public Vector3 Target { get; set; }

            public override void Enter(AugmentedEntity entity)
            {
            }

            public override void Execute(AugmentedEntity entity)
            {
            }

            public override void Exit(AugmentedEntity entity)
            {
            }

            public override void Reset(AugmentedEntity entity)
            {
            }
        }

        public class IdleState : State<AugmentedEntity>
        {
            public override void Enter(AugmentedEntity entity)
            {
            }

            public override void Execute(AugmentedEntity entity)
            {
            }

            public override void Exit(AugmentedEntity entity)
            {
            }

            public override void Reset(AugmentedEntity entity)
            {
            }
        }
    }
}
