using Microsoft.Xna.Framework.Graphics;

namespace DavidFidge.MonoGame.Core.Interfaces.Graphics
{
    public interface ITextureDictionary
    {
        Texture2D GetTexture(string textureName);
    }
}