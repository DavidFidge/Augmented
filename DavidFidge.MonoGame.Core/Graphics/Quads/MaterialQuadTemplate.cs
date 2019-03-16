using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics.Quads
{
    public class MaterialQuadTemplate : BaseQuadTemplate
    {
        private Color _colour;

        public Color Color
        {
            get => _colour;

            set
            {
                _colour = value;

                if (_basicEffect != null)
                    _basicEffect.DiffuseColor = _colour.ToVector3();
            }
        }

        public MaterialQuadTemplate(IGameProvider gameProvider) : base(gameProvider)
        {
        }

        public void LoadContent(float width, float height, Color colour)
        {
            LoadContent(width, height, colour, Vector3.Zero);
            _colour = colour;
        }

        public void LoadContent(Vector2 size, Color colour)
        {
            LoadContent(size.X, size.Y, colour);
        }

        public void LoadContent(
            float width,
            float height,
            Color colour,
            Vector3 displacement)
        {
            LoadContent(width, height, displacement);
            _colour = colour;
            _basicEffect = _gameProvider.Game.EffectCollection.BuildMaterialEffect(colour);
        }

        public void LoadContent(
            Vector2 size,
            Color colour,
            Vector3 displacement)
        {
            LoadContent(size.X, size.Y, colour, displacement);
        }
    }
}