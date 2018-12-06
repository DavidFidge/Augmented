using Augmented.Interfaces;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics.Camera
{
    public class AugmentedGameWorld : IAugmentedGameWorld
    {
        private readonly TestQuad _testQuad;

        public AugmentedGameWorld(TestQuad testQuad)
        {
            _testQuad = testQuad;
        }

        public void Initialise()
        {
            _testQuad.Initialise();
        }

        public void Draw(Matrix projection, Matrix view)
        {
            _testQuad.Draw(projection, view);
        }
    }
}