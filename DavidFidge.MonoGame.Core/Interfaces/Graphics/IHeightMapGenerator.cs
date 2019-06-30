using DavidFidge.MonoGame.Core.Graphics.Terrain;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Interfaces.Graphics
{
    public interface IHeightMapGenerator
    {
        HeightMapGenerator Randomise(int patchX, int patchY);

        HeightMapGenerator Hill(
            Vector2 relativeCentre,
            Vector2 relativeSize,
            int height,
            HeightMapGenerator.HillOptions hillOptions = HeightMapGenerator.HillOptions.ReplaceIfHeigher
        );

        HeightMapGenerator Ridge(
            Vector2 relativeStart,
            Vector2 relativeEnd,
            Vector2 relativeSize,
            int height
        );

        HeightMapGenerator CreateHeightMap(int width, int length);
    }
}