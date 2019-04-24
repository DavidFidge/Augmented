using Augmented.Graphics.Models;
using Augmented.Graphics.TerrainSpace;
using Augmented.Interfaces;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics
{
    public class AugmentedGameWorld : IAugmentedGameWorld
    {
        private readonly Terrain _terrain;
        private readonly AugmentedModel _augmentedModel;

        public AugmentedGameWorld(
            Terrain terrain,
            AugmentedModel augmentedModel
            )
        {
            _terrain = terrain;
            _augmentedModel = augmentedModel;
            _terrain.WorldTransform.ChangeScale(new Vector3(1f, 1f, 0.5f));
        }

        public void LoadContent()
        {
            _terrain.LoadContent();
            _augmentedModel.LoadContent();
        }

        public void Draw(Matrix view, Matrix projection)
        {
            _augmentedModel.Draw(view, projection);
            _terrain.Draw(view, projection);
        }
    }
}