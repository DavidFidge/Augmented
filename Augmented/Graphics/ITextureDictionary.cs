using Microsoft.Xna.Framework.Graphics;

namespace Augmented.Graphics
{
    public interface ITextureDictionary
    {
        Texture2D GetTexture(string textureName);
    }
}