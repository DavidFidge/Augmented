using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DavidFidge.MonoGame.Core.Graphics.Quads
{
    public class TexturedQuadTemplate : BaseQuadTemplate
    {
        public TexturedQuadTemplate(IGameProvider gameProvider)
           : base(gameProvider)
        {
            _gameProvider = gameProvider;
        }

        public void LoadContent(float width, float height, string textureName)
        {
            LoadContent(width, height);

            Effect = _gameProvider.Game.EffectCollection.BuildMaterialTextureEffect(textureName);
        }
        
        public void LoadContent(float width, float height, string textureName, string effectName)
        {
            LoadContent(width, height);

            Effect = _gameProvider.Game.EffectCollection[effectName];

            Effect.Parameters["Texture"].SetValue(_gameProvider.Game.Content.Load<Texture2D>(textureName));
        }

        public void LoadContent(Vector2 size, string textureName)
        {
            LoadContent(size.X, size.Y, textureName);
        }
    }
}