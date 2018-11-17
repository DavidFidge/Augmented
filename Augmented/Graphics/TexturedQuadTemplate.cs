using DavidFidge.MonoGame.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Augmented.Graphics
{
    public class TexturedQuadTemplate : BaseQuadTemplate
    {
        private readonly ITextureDictionary _textureDictionary;
        private string _textureName;

        public TexturedQuadTemplate(IGameProvider gameProvider, ITextureDictionary textureDictionary)
           : base(gameProvider)
        {
            _textureDictionary = textureDictionary;
            _gameProvider = gameProvider;
        }

        public void Initialise(string textureName, float width, float height)
        {
            Initialise(width, height);
            _textureName = textureName;
            LoadBasicEffect();
        }

        public void Initialise(string textureName, Vector2 size)
        {
            Initialise(textureName, size.X, size.Y);
        }

        private void LoadBasicEffect()
        {
            _basicEffect = new BasicEffect(_gameProvider.Game.GraphicsDevice)
            {
                LightingEnabled = false,
                TextureEnabled = true
            };
        }

        protected override void PrepareBasicEffectForDraw()
        {
            _basicEffect.Texture = _textureDictionary.GetTexture(_textureName);
        }
    }
}