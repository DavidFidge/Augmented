using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics
{
    public class TestTexturedQuad : IWorldTransformable
    {
        private readonly TexturedQuadTemplate _texturedQuadTemplate;
        public IWorldTransform WorldTransform { get; private set; }

        public TestTexturedQuad(TexturedQuadTemplate texturedQuadTemplate) 
        {
            _texturedQuadTemplate = texturedQuadTemplate;
            WorldTransform = new SimpleWorldTransform();
        }

        public void LoadContent()
        {
            _texturedQuadTemplate.LoadContent(100f, 100f, Constants.GrassTexture);
        }

        public void Draw(Matrix projection, Matrix view)
        {
            _texturedQuadTemplate.Draw(WorldTransform.World, view, projection);
        }
    }
}