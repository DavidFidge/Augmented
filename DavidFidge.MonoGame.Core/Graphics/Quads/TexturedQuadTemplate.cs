using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;

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
            _gameProvider.Game.EffectCollection.BuildTextureEffect(textureName);
        }

        public void LoadContent(Vector2 size, string textureName)
        {
            LoadContent(size.X, size.Y, textureName);
        }
    }
}