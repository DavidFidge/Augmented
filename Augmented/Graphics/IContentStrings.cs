using DavidFidge.MonoGame.Core.Graphics;

namespace Augmented.Graphics
{
    public interface IContentStrings : ICoreContent
    {
        string WoodTexture { get; set; }
        string GrassTexture { get; set; }
        string SelectionEffect { get; set; }
    }
}