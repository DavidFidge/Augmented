using DavidFidge.MonoGame.Core.Components;

using Microsoft.Xna.Framework;

using NGenerics.DataStructures.Trees;

namespace DavidFidge.MonoGame.Core.Interfaces.Graphics
{
    public interface ISceneGraph
    {
        GeneralTree<Entity> Root { get; set; }
        void Draw(Matrix cameraView, Matrix cameraProjection);
        void LoadContent();
    }
}