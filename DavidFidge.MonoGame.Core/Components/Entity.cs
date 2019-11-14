using System;

using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

namespace DavidFidge.MonoGame.Core.Components
{
    public abstract class Entity : ITransformable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ITransform WorldTransform { get; set; } = new EntityTransform();
        public ITransform LocalTransform { get; set; } = new EntityTransform();
    }
}
