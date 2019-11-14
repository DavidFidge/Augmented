using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;

using NGenerics.Patterns.Visitor;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public class SceneGraph : ISceneGraph
    {
        private SceneGraphNode Root { get; set; }
        private Dictionary<Entity, SceneGraphNode> _sceneGraphNodes = new Dictionary<Entity, SceneGraphNode>();

        public void Initialise(Entity root)
        {
            _sceneGraphNodes = new Dictionary<Entity, SceneGraphNode>();
            Root = new SceneGraphNode(root);
            _sceneGraphNodes[root] = Root;
        }

        public void LoadContent()
        {
            Root.Node.BreadthFirstTraversal(new LoadContentVisitor());
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Root.Node.BreadthFirstTraversal(new DrawVisitor(view, projection, this));
        }

        public void DeselectAll()
        {
            Root.Node.BreadthFirstTraversal(new ActionVisitor<Entity>(
                e =>
                {
                    if (e is ISelectable deselect)
                        deselect.IsSelected = false;
                }));
        }

        public void Add(Entity entity, Entity parent)
        {
            var node = new SceneGraphNode(entity);

            _sceneGraphNodes[entity] = node;
            _sceneGraphNodes[parent].AddChild(node);
        }

        public void Remove(Entity entity)
        {
            if (Root.Node.Data == entity)
                throw new Exception("Cannot remove root node");

            var node = Root.Node.FindNode(n => n == entity);

            var visitor = new BreadthFirstNodeCollectionVisitor();
            node.BreadthFirstTraversal(visitor);

            foreach (var entityUnderRemovedNode in visitor.VisitedEntities)
            {
                _sceneGraphNodes.Remove(entityUnderRemovedNode);
            }

            node.Parent.Remove(node);
        }

        public Entity Select(Ray ray)
        {
            var selectVisitor = new SelectVisitor(ray, this);
            
            Root.Node.BreadthFirstTraversal(selectVisitor);

            var selectedEntity = selectVisitor.SelectedEntities.OrderBy(e => e.Distance);

            if (selectedEntity.Any())
                return selectedEntity.First().Entity;

            return null;
        }

        public List<Entity> GetEntitiesByBreadthFirstSearch()
        {
            var breadthFirstNodeCollectionVisitor = new BreadthFirstNodeCollectionVisitor();

            Root.Node.BreadthFirstTraversal(breadthFirstNodeCollectionVisitor);

            return breadthFirstNodeCollectionVisitor.VisitedEntities;
        }

        public Matrix GetWorldTransform(Entity entity)
        {
            var parent = _sceneGraphNodes[entity].Node.Parent;

            if (parent == null)
                return entity.WorldTransform.Transform;

            return GetWorldTransform(parent.Data) * entity.WorldTransform.Transform;
        }

        public Matrix GetWorldTransformWithLocalTransform(Entity entity)
        {
            var worldTransform = GetWorldTransform(entity);

            return entity.LocalTransform.Transform * worldTransform;
        }
    }

    public class BreadthFirstNodeCollectionVisitor: IVisitor<Entity>
    {
        public List<Entity> VisitedEntities { get; set; } = new List<Entity>();

        public void Visit(Entity entity)
        {
            VisitedEntities.Add(entity);
        }

        public bool HasCompleted => false;
    }

    public struct SelectedEntity
    {
        public Entity Entity;
        public float Distance;
    }
}