using Augmented.Graphics.TerrainSpace;
using Augmented.Interfaces;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics
{
    public class AugmentedGameWorld : IAugmentedGameWorld
    {
        private readonly Terrain _terrain;

        public AugmentedGameWorld(Terrain terrain)
        {
            _terrain = terrain;
            _terrain.WorldTransform.ChangeScale(new Vector3(1f, 1f, 0.5f));
        }

        public void LoadContent()
        {
            _terrain.LoadContent();
        }

        public void Draw(Matrix projection, Matrix view)
        {

            _terrain.Draw(view, projection);
        }
    }
}