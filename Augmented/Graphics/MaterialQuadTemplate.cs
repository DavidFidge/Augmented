using DavidFidge.MonoGame.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Augmented.Graphics
{
    public class MaterialQuadTemplate : BaseQuadTemplate
    {
        private Color _colour;

        public MaterialQuadTemplate(IGameProvider gameProvider) : base(gameProvider)
        {
        }

        public void Initialise(float width, float height, Color colour)
        {
            Initialise(width, height);
            _colour = colour;
            LoadBasicEffect();
        }

        public void Initialise(Vector2 size, Color colour)
        {
            Initialise(size.X, size.Y, colour);
        }

        public void Initialise(
            float width,
            float height,
            Color colour,
            Vector3 displacemet)
        {
            Initialise(width, height, displacemet);
            _colour = colour;
            LoadBasicEffect();
        }

        public void Initialise(
            Vector2 size,
            Color colour,
            Vector3 displacement)
        {
            Initialise(size.X, size.Y, colour, displacement);
        }

        public void SetColour(Color colour)
        {
            if (_basicEffect != null)
                _basicEffect.DiffuseColor = colour.ToVector3();
        }

        public Color GetColour()
        {
            return _colour;
        }

        private void LoadBasicEffect()
        {
            _basicEffect = new BasicEffect(_gameProvider.Game.GraphicsDevice)
            {
                DiffuseColor = _colour.ToVector3(),
                Alpha = _colour.A / 255.0f
            };
        }
    }
}