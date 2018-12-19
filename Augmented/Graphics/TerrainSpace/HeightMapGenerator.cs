using System;

using Augmented.Graphics.Camera;

namespace Augmented.Graphics.TerrainSpace
{
    public class HeightMapGenerator : IHeightMapStore
    {
        public int[,] GetHeightMap()
        {
            var random = new Random();

            var result = new int[10, 10];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    result[i, j] = random.Next(0, 20);
                }
            }

            return result;
        }
    }
}