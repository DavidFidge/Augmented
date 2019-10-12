using DavidFidge.MonoGame.Core.Components;

using Microsoft.Xna.Framework;

using NGenerics.Patterns.Visitor;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public class DrawVisitor : IVisitor<Entity>
    {
        private readonly Matrix _view;
        private readonly Matrix _projection;

        public DrawVisitor(Matrix view, Matrix projection)
        {
            _view = view;
            _projection = projection;
        }

        public void Visit(Entity entity)
        {
            if (entity is IDrawable drawable)
            {
                drawable.Draw(_view, _projection);
            }
        }

        public bool HasCompleted => false;
    }
}