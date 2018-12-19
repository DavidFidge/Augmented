using Augmented.Graphics.TerrainSpace;
using Augmented.Interfaces;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics
{
    public class AugmentedGameWorld : IAugmentedGameWorld
    {
        private readonly TestQuad _testQuad;
        private readonly TestTexturedQuad _testTexturedQuad;
        private Terrain _terrain;

        public AugmentedGameWorld(
            TestQuad testQuad,
            TestTexturedQuad testTexturedQuad,
            Terrain terrain)
        {
            _terrain = terrain;
            _testQuad = testQuad;
            _testTexturedQuad = testTexturedQuad;
        }

        public void LoadContent()
        {
            _testQuad.LoadContent();
            _terrain.LoadContent();
            _testTexturedQuad.LoadContent();
            _testQuad.WorldTransform.ChangeTranslationRelative(new Vector3(0f, 100f, 0f));
            _testTexturedQuad.WorldTransform.ChangeTranslationRelative(new Vector3(0f, 200f, 0f));
        }

        public void Draw(Matrix projection, Matrix view)
        {
            _testQuad.Draw(projection, view);
            _testTexturedQuad.Draw(projection, view);
            _terrain.Draw(projection, view);
        }
    }

    public interface IHeightMapStore
    {
        int[,] GetHeightMap();
    }
}