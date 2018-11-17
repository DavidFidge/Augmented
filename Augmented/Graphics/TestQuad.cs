using System;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics
{
    public class TestQuad : IWorldTransformable
    {
        private readonly MaterialQuadTemplate _materialQuadTemplate;
        public IWorldTransform WorldTransform { get; private set; }

        public TestQuad(MaterialQuadTemplate materialQuadTemplate) 
        {
            _materialQuadTemplate = materialQuadTemplate;
            WorldTransform = new SimpleWorldTransform();
        }

        public void Initialise()
        {
            _materialQuadTemplate.Initialise(100f, 100f, Color.Red);
        }

        public void Draw(Matrix projection, Matrix view)
        {
            _materialQuadTemplate.Draw(WorldTransform.World, view, projection);
        }
    }
}