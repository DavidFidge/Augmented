using System.Collections.Generic;

using DavidFidge.MonoGame.Core.Components;

using Microsoft.Xna.Framework;

using NGenerics.Patterns.Visitor;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public class SelectVisitor : IVisitor<Entity>
    {
        private readonly Ray _ray;

        public List<SelectedEntity> SelectedEntities { get; set; } = new List<SelectedEntity>();

        public SelectVisitor(Ray ray)
        {
            _ray = ray;
        }

        public void Visit(Entity entity)
        {
            if (entity is ISelectable selectable)
            {
                var intersectDistance = selectable.Intersects(_ray);

                if (intersectDistance != null)
                    SelectedEntities.Add(new SelectedEntity { Entity = entity, Distance = intersectDistance.Value });
            }
        }

        public bool HasCompleted { get; } = false;
    }
}