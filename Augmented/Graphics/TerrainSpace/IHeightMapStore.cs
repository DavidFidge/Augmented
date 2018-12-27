using System;
using System.Collections.Generic;
using System.Linq;

namespace Augmented.Graphics.TerrainSpace
{
    public interface IHeightMapStore
    {
        int[,] GetHeightMap();
    }
}