using DavidFidge.MonoGame.Core.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;

using NGenerics.DataStructures.Trees;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public class SceneGraph : ISceneGraph
    {
        public GeneralTree<Entity> Root { get; set; }

        public void LoadContent()
        {
            Root.BreadthFirstTraversal(new LoadContentVisitor());
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Root.BreadthFirstTraversal(new DrawVisitor(view, projection));
        }
    }
}