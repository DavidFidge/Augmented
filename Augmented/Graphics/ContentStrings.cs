using DavidFidge.MonoGame.Core.Graphics;

namespace Augmented.Graphics
{
    public class ContentStrings : IContentStrings
    {
        public ContentStrings()
        {
            SelectionEffect = @"Effects\Selection";
            SelectionTexture = @"Sprites\Selection";
            GrassTexture = @"Terrain\Grass";
            WoodTexture = @"Terrain\Wood";
        }

        public string WoodTexture { get; set; }
        public string GrassTexture { get; set; }
        public string SelectionEffect { get; set; }
        public string SelectionTexture { get; set; }
    }
}