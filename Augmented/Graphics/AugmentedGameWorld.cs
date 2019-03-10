using Augmented.Graphics.Models;
using Augmented.Graphics.TerrainSpace;
using Augmented.Interfaces;

using DavidFidge.MonoGame.Core.Graphics.Cylinder;
using DavidFidge.MonoGame.Core.Graphics.Trees;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics
{
    public class AugmentedGameWorld : IAugmentedGameWorld
    {
        private readonly Terrain _terrain;
        private readonly AugmentedModel _augmentedModel;
        private readonly Cylinder _cylinder;
        private readonly Tree _tree;

        public AugmentedGameWorld(
            Terrain terrain,
            AugmentedModel augmentedModel,
            Cylinder cylinder,
            Tree tree
            )
        {
            _terrain = terrain;
            _augmentedModel = augmentedModel;
            _cylinder = cylinder;
            _tree = tree;
            _terrain.WorldTransform.ChangeScale(new Vector3(1f, 1f, 0.5f));
        }

        public void LoadContent()
        {
            _terrain.LoadContent();
            _augmentedModel.LoadContent();
            _cylinder.LoadContent(5f, 20f);
            _tree.LoadContent(Constants.WoodTexture);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            _augmentedModel.Draw(view, projection);
            _cylinder.Draw(view, projection);
            _tree.Draw(view, projection);
            _terrain.Draw(view, projection);
        }
    }
}