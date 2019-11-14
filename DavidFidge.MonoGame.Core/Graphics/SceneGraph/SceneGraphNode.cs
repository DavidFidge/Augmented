using DavidFidge.MonoGame.Core.Components;

using NGenerics.DataStructures.Trees;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public class SceneGraphNode
    {
        public GeneralTree<Entity> Node { get; set; }

        public SceneGraphNode(Entity entity)
        {
            Node = new GeneralTree<Entity>(entity);
        }

        public void AddChild(SceneGraphNode child)
        {
            Node.Add(child.Node);
        }
    }
}