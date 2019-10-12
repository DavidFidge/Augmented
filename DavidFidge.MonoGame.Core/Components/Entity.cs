using System;

using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using NGenerics.DataStructures.Trees;

namespace DavidFidge.MonoGame.Core.Components
{
    public abstract class Entity : IWorldTransformable
    {
        protected Entity()
        {
            SceneNode = new GeneralTree<Entity>(this);
        }

        public GeneralTree<Entity> SceneNode { get; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public IWorldTransform WorldTransform { get; set; }
    }
}
