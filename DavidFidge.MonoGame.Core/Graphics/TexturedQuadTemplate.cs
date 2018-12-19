using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public class TexturedQuadTemplate : BaseQuadTemplate
    {
        private string _textureName;

        public TexturedQuadTemplate(IGameProvider gameProvider)
           : base(gameProvider)
        {
            _gameProvider = gameProvider;
        }

        public void LoadContent(float width, float height, string textureName)
        {
            LoadContent(width, height);
            _textureName = textureName;
            LoadBasicEffect();
        }

        public void LoadContent(Vector2 size, string textureName)
        {
            LoadContent(size.X, size.Y, textureName);
        }

        private void LoadBasicEffect()
        {
            _basicEffect = new BasicEffect(_gameProvider.Game.GraphicsDevice)
            {
                LightingEnabled = false,
                TextureEnabled = true,
                Texture = _gameProvider.Game.Content.Load<Texture2D>(_textureName)
            };
        }
    }
}