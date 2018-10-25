using System;

using Microsoft.Xna.Framework;

using MonoGame.Extended;
using MonoGame.Extended.Sprites;

namespace Augmented.Entities
{

    public class EntityGraphic
    {
        public Sprite Sprite { get; set; }

        public EntityGraphic()
        {
        }

        public void Draw()
        {
        }

    }



    public class Augmented : Entity
    {
        public Augmented()
        {

        }
    }

    public class Enemy : Entity
    {

    }

    public class Entity : IMovable, IGameComponent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Vector2 Position { get; set; } = Vector2.Zero;

        public EntityGraphic EntityGraphic { get; set; }

        public void Initialize()
        {
        }
    }
}
